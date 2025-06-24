using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Credits { get; set; }

    public int InstructorId { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Instructor Instructor { get; set; } = null!;
}
