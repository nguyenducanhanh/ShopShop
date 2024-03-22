using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? CustomerName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? HowToPay { get; set; }

    public double? TransportFee { get; set; }

    public int? StatusId { get; set; }

    public string? Note { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetailId> OrderDetailIds { get; set; } = new List<OrderDetailId>();

    public virtual Status? Status { get; set; }
}
