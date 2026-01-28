using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//updated 05.01.26
namespace TaskManager.Application.DTOs.Roles;


public class RoleDto
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;
}

