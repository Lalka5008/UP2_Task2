using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class Bid
{
    public int BidId { get; set; }

    public int? СourseId { get; set; }

    public int? UserId { get; set; }

    public DateOnly? BidData { get; set; }

    public int? BidStatusId { get; set; }

    public int? Seats { get; set; }

    public int? TotalPrice { get; set; }

    public string? Comment { get; set; }

    public virtual BidStatus? BidStatus { get; set; }

    public virtual User? User { get; set; }

    public virtual Сourse? Сourse { get; set; }
}
