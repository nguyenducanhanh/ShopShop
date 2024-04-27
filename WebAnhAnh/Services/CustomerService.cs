using WebAnhAnh.Models;

namespace WebAnhAnh.Services
{
    
    public class CustomerService
    {
        private readonly ShopShopContext _db;

        public CustomerService(ShopShopContext db)
        {
            _db = db;
        }

        public int GetRegisteredAccountsCount()
        {
            return _db.Customers.Count();
        }

        public int GetOrdersCount()
        {
            return _db.Orders.Count();
        }
        public int GetOrderDetailIdsCount()
        {
            return _db.OrderDetailIds.Sum(od => od.Quantity ?? 0);
        }

        public double GetTotalMoneySum()
        {
            return _db.OrderDetailIds.Sum(od => od.Price ?? 0);
        }






    }

}
