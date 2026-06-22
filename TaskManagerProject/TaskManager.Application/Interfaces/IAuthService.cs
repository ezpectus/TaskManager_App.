using TaskManager.Application.DTOs.Auth;

namespace TaskManager.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<LoginResponse?> RefreshAsync(string refreshToken, CancellationToken ct);
}
