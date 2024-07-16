using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? ProductId { get; set; }

    public string? CustomerId { get; set; }

    public string? CommentText { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
