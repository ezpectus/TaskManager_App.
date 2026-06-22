using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManager.Application.DTOs.Users;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
[Authorize]
[EnableRateLimiting("api")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "id" })]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var user = await _service.GetByIdAsync(id, ct);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(CreateUserRequest dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UserDto dto, CancellationToken ct)
    {
        var ok = await _service.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Update user profile (username and email)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="dto">Updated username and email</param>
    /// <param name="ct">Cancellation token</param>
    [HttpPut("{id:guid}/profile")]
    public async Task<IActionResult> UpdateProfile(Guid id, UpdateUserRequest dto, CancellationToken ct)
    {
        var ok = await _service.UpdateProfileAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Change user password (requires current password verification)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="request">Current password + new password</param>
    /// <param name="ct">Cancellation token</param>
    [HttpPost("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken ct)
    {
        if (id != request.UserId)
            return BadRequest("User ID mismatch");
        var ok = await _service.ChangePasswordAsync(request, ct);
        return ok ? NoContent() : BadRequest("Current password is incorrect or user not found");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
