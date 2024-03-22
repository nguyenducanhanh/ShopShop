using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public string? Describe { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
