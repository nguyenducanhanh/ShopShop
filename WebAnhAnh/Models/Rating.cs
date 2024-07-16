using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int? ProductId { get; set; }

    public string? CustomerId { get; set; }

    public int? Score { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
