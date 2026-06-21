using System;

namespace TaskManager.Application.DTOs.Roles;

public class RoleDto
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;
}
