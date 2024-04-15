using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopShopContext db;

        public ProductController(ShopShopContext conetxt)
        {
            db = conetxt;
        }


     
        public IActionResult Index(int? category)
        {
            var products = db.Products.AsQueryable();

            if (category.HasValue)
            {
                products = products.Where(p => p.CategoryId == category.Value);
            }

            var result = products.Select(p => new ProductsRepository
            {
                ProductID = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price ?? 0,
                Image = p.Image ?? "",
                Image1 = p.Image1 ?? "",
                Image2 = p.Image2 ?? "",
                Describe = p.Describe ?? "",
                CategoryName = p.Category.CategoryName,
            });
            return View(result);
        }

        public IActionResult Search(string? query)
        {
            var products = db.Products.AsQueryable();

            if (query != null)
            {
                products = products.Where(p => p.ProductName.Contains(query));
            }

            var result = products.Select(p => new ProductsRepository
            {
                ProductID = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price ?? 0,
                Image = p.Image ?? "",
                Describe = p.Describe ?? "",
                CategoryName = p.Category.CategoryName,
            });
            return View(result);
        }

        public IActionResult DetailProduct(int id)
        {
            var data = db.Products
                .Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == id);
            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }

           var result = new DetailProductsRepository
           {
              ProductID = data.ProductId,
               ProductName = data.ProductName,
              Price = data.Price ?? 0,
               Image = data.Image ?? string.Empty,
               Image1 = data.Image1 ?? string.Empty,
               Image2 = data.Image2 ?? string.Empty,
               Describe = data.Describe ?? string.Empty,
              CategoryName = data.Category.CategoryName,
              SoLuongTon = 10,//tính sau
              DiemDanhGia = 5,//check sau
          };
            return View(result);
        }
    }
}
