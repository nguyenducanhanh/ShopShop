using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.ViewComponents
{
    public class NewProductViewComponent : ViewComponent
    {
        private readonly ShopShopContext db;

        public NewProductViewComponent(ShopShopContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var newestProduct = db.Products
                 .OrderByDescending(p => p.ProductId) // Sắp xếp theo ngày tạo mới nhất
                 .Include(p => p.Category)
                 .FirstOrDefault();

            if (newestProduct == null)
            {
                // Không có sản phẩm nào trong cơ sở dữ liệu
                return View("Empty");
            }

            var result = new ProductsRepository
            {
                ProductID = newestProduct.ProductId,
                ProductName = newestProduct.ProductName,
                Price = newestProduct.Price ?? 0,
                Image = newestProduct.Image ?? string.Empty,
                Describe = newestProduct.Describe ?? string.Empty,
                CategoryName = newestProduct.Category.CategoryName,
            };

            return View(result);

        }

       
    }
}
