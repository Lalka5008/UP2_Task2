using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string? TeacherType { get; set; }

    public string? Discription { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<Сourse> Сourses { get; set; } = new List<Сourse>();
}
