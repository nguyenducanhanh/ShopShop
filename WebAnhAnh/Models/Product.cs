using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? CategoryId { get; set; }

    public double? Price { get; set; }

    public string? Image { get; set; }

    public DateTime? DateOfManufacture { get; set; }

    public double? Discount { get; set; }

    public string? Describe { get; set; }

    public string? SupplierId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetailId> OrderDetailIds { get; set; } = new List<OrderDetailId>();

    public virtual Supplier? Supplier { get; set; }
}
