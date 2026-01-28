using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
// 28.01.26 v 1.01


namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/user-roles")]
public class UserRolesController : ControllerBase
{
    private readonly IUserRoleService _service;

    public UserRolesController(IUserRoleService service)
    {
        _service = service;
    }

    [HttpPost]
   // public async Task<IActionResult> Add(Guid userId, Guid roleId, CancellationToken ct)
   // {
      //  await _service.AddAsync(userId, roleId, ct);
      //  return NoContent();
   // }


    public async Task<IActionResult> Add(Guid userId, Guid roleId, CancellationToken ct)
    {
        await _service.AssignRoleAsync(userId, roleId, ct);
        return NoContent();
    }
}

