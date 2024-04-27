using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.ViewComponents
{
    public class MaxProductViewComponent : ViewComponent
    {
        private readonly ShopShopContext db;

        public MaxProductViewComponent(ShopShopContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var highestPriceProduct = db.Products
                 .OrderByDescending(p => p.Price) // Sắp xếp theo giá cao nhất
                 .Include(p => p.Category)
                 .FirstOrDefault();

            if (highestPriceProduct == null)
            {
                // Không có sản phẩm nào trong cơ sở dữ liệu
                return View("Empty");
            }

            var result = new ProductsRepository
            {
                ProductID = highestPriceProduct.ProductId,
                ProductName = highestPriceProduct.ProductName,
                Price = highestPriceProduct.Price ?? 0,
                Image = highestPriceProduct.Image ?? string.Empty,
                Describe = highestPriceProduct.Describe ?? string.Empty,
                CategoryName = highestPriceProduct.Category.CategoryName,
            };

            return View(result);
        }



    }
}
