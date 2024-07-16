using System.Collections.Generic;
using System.Linq;
using WebAnhAnh.Models;

namespace WebAnhAnh.Repository
{
    public class CommentRepository
    {
        public int CommentId { get; set; }

        public int? ProductId { get; set; }

        public string? CustomerId { get; set; }

        public string? CommentText { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual Product? Product { get; set; }

            public string? CustomerName { get; set; }
  

}
}
