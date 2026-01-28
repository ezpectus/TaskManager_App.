using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26

namespace TaskManager.Application.DTOs.Users;
using TaskManager.Application.DTOs.Roles;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public List<RoleDto> Roles { get; set; } = new();
}



