using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/user-roles")]
[ApiVersion("1.0")]
[Authorize]
public class UserRolesController : ControllerBase
{
    private readonly IUserRoleService _service;

    public UserRolesController(IUserRoleService service)
    {
        _service = service;
    }

    [HttpPost("{userId:guid}/{roleId:guid}")]
    public async Task<IActionResult> Add(Guid userId, Guid roleId, CancellationToken ct)
    {
        await _service.AssignRoleAsync(userId, roleId, ct);
        return NoContent();
    }

    [HttpDelete("{userId:guid}/{roleId:guid}")]
    public async Task<IActionResult> Remove(Guid userId, Guid roleId, CancellationToken ct)
    {
        await _service.RemoveRoleAsync(userId, roleId, ct);
        return NoContent();
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetUserRoles(Guid userId, CancellationToken ct)
        => Ok(await _service.GetUserRolesAsync(userId, ct));
}
