using System;
using System.Collections.Generic;
using TaskManager.Application.DTOs.Roles;

namespace TaskManager.Application.DTOs.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public List<RoleDto> Roles { get; set; } = new();
}

