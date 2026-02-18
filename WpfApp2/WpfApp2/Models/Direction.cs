using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class Direction
{
    public int DirectionId { get; set; }

    public string? DirectionName { get; set; }

    public string? Discrption { get; set; }

    public virtual ICollection<Сourse> Сourses { get; set; } = new List<Сourse>();
}
