using WebAnhAnh.Models;

namespace WebAnhAnh.Repository
{
    public class OrderDetailRepository
    {
        public int OrderDetailId1 { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }

        public double? Discount { get; set; }

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }
        public double TotalMoney => (double) (Price * Quantity);
    }
}
