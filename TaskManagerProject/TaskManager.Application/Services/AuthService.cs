using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Auth;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserService userService,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userService = userService;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var identifier = request.Email.Trim();

        User? user;
        if (identifier.Contains('@'))
        {
            user = await _userRepository.GetByEmailAsync(identifier, ct);
        }
        else
        {
            user = await _userRepository.GetByUsernameAsync(identifier, ct);
        }

        if (user == null)
        {
            _logger.LogWarning("Failed login attempt: user '{Identifier}' not found", identifier);
            return null;
        }

        var valid = await _userService.ValidateCredentialsAsync(user.Email, request.Password, ct);
        if (!valid)
        {
            _logger.LogWarning("Failed login attempt: invalid password for user '{Username}' ({Email})", user.Username, user.Email);
            return null;
        }

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        var token = GenerateJwtToken(user.Id, user.Username, user.Email, roles);
        var refreshTokenStr = GenerateRefreshTokenString();
        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenStr,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("User '{Username}' ({Email}) logged in successfully. Roles: [{Roles}]",
            user.Username, user.Email, string.Join(", ", roles));

        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshTokenStr,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Username = user.Username
        };
    }

    public async Task<LoginResponse?> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, ct);
        if (storedToken == null)
        {
            _logger.LogWarning("Refresh token not found in database");
            return null;
        }

        if (!storedToken.IsActive)
        {
            _logger.LogWarning("Refresh token for user {UserId} is revoked or expired", storedToken.UserId);
            return null;
        }

        var user = await _userRepository.GetByIdAsync(storedToken.UserId, ct);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found during token refresh", storedToken.UserId);
            return null;
        }

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

        var newToken = GenerateJwtToken(user.Id, user.Username, user.Email, roles);
        var newRefreshTokenStr = GenerateRefreshTokenString();

        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.ReplacedByToken = newRefreshTokenStr;

        var newRefreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshTokenStr,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.UpdateAsync(storedToken, ct);
        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Refreshed token for user '{Username}' ({Email})", user.Username, user.Email);

        return new LoginResponse
        {
            Token = newToken,
            RefreshToken = newRefreshTokenStr,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Username = user.Username
        };
    }

    public async Task<bool> RevokeAsync(string refreshToken, CancellationToken ct)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, ct);
        if (storedToken == null || !storedToken.IsActive)
            return false;

        storedToken.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(storedToken, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Refresh token revoked for user {UserId}", storedToken.UserId);
        return true;
    }

    private string GenerateRefreshTokenString()
    {
        var randomBytes = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private string GenerateJwtToken(Guid userId, string username, string email, List<string> roles)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured");
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "TaskManager";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "TaskManager";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, username),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
