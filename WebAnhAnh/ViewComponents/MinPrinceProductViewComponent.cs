using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.ViewComponents
{
    public class MinProductViewComponent : ViewComponent
    {
        private readonly ShopShopContext db;

        public MinProductViewComponent(ShopShopContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var lowestPriceProduct = db.Products
                 .OrderBy(p => p.Price) // Sắp xếp theo giá nhỏ nhất
                 .Include(p => p.Category)
                 .FirstOrDefault();

            if (lowestPriceProduct == null)
            {
                // Không có sản phẩm nào trong cơ sở dữ liệu
                return View("Empty");
            }

            var result = new ProductsRepository
            {
                ProductID = lowestPriceProduct.ProductId,
                ProductName = lowestPriceProduct.ProductName,
                Price = lowestPriceProduct.Price ?? 0,
                Image = lowestPriceProduct.Image ?? string.Empty,
                Describe = lowestPriceProduct.Describe ?? string.Empty,
                CategoryName = lowestPriceProduct.Category.CategoryName,
            };

            return View(result);
        }




    }
}
