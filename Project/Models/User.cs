using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
