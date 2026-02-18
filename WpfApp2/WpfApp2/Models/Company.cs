using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
