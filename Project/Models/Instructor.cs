using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Instructor
{
    public int InstructorId { get; set; }

    public int UserId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual User User { get; set; } = null!;
}
