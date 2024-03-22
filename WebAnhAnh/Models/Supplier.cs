using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Supplier
{
    public string SupplierId { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? Logo { get; set; }

    public string? ContactName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Describe { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
