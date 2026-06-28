using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManager.Application.DTOs.Auth;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
[AllowAnonymous]
[EnableRateLimiting("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticate user and receive JWT token + refresh token
    /// </summary>
    /// <param name="request">Login credentials (email or username + password)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>JWT token, refresh token, user info</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request, ct);
        return result == null ? Unauthorized() : Ok(result);
    }

    /// <summary>
    /// Refresh expired JWT token using a valid refresh token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>New JWT token + new refresh token</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken ct)
    {
        var result = await _authService.RefreshAsync(request.RefreshToken, ct);
        return result == null ? Unauthorized() : Ok(result);
    }

    /// <summary>
    /// Revoke a refresh token (logout)
    /// </summary>
    /// <param name="request">Refresh token to revoke</param>
    /// <param name="ct">Cancellation token</param>
    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke(RefreshTokenRequest request, CancellationToken ct)
    {
        var ok = await _authService.RevokeAsync(request.RefreshToken, ct);
        return ok ? NoContent() : NotFound();
    }
}
