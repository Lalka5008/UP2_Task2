using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class Сourse
{
    public int СourseId { get; set; }

    public string? СourseName { get; set; }

    public int? DirectionId { get; set; }

    public int? Long { get; set; }

    public DateOnly? StartDate { get; set; }

    public int? Price { get; set; }

    public int? TeacherId { get; set; }
    public string ImageFullPath => !string.IsNullOrWhiteSpace(Image) ? $"Images/{Image}" : "Images/picture.png";
    public int? Free { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Direction? Direction { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
