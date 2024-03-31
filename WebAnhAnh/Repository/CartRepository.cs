namespace WebAnhAnh.Repository
{
    public class CartRepository
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPayment => Quantity * Price;
    }
}
