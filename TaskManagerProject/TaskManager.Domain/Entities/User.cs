using System;
using System.Collections.Generic;

namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public List<UserRole> UserRoles { get; set; } = new();
    public List<TaskItem> Tasks { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}





