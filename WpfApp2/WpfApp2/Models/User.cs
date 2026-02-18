using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? RoleId { get; set; }

    public string? Name { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int? CompanyId { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Company? Company { get; set; }

    public virtual Role? Role { get; set; }
}
