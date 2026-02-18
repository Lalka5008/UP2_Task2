using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class BidStatus
{
    public int BidStatusId { get; set; }

    public string? BidStatusName { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
