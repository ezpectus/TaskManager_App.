namespace TaskManager.Application.DTOs.Auth;

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
}
