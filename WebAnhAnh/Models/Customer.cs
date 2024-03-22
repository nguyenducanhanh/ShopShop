using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string? PassWord { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public bool Effect { get; set; }

    public int Role { get; set; }

    public string? RandomKey { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
