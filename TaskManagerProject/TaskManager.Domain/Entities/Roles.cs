using System;
using System.Collections.Generic;

namespace TaskManager.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;

    public List<UserRole> UserRoles { get; set; } = new();
}
