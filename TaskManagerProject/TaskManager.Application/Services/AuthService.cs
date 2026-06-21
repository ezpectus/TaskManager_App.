using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Auth;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserService userService,
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _userService = userService;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var valid = await _userService.ValidateCredentialsAsync(request.Email, request.Password, ct);
        if (!valid) return null;

        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user == null) return null;

        var token = GenerateJwtToken(user.Id, user.Username, user.Email);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Username = user.Username
        };
    }

    private string GenerateJwtToken(Guid userId, string username, string email)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured");
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "TaskManager";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "TaskManager";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
