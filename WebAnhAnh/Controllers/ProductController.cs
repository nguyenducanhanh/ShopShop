using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;
using WebAnhAnh.Helpers;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopShopContext db;
        private readonly CommentRepository commentRepo;
        
        public ProductController(ShopShopContext context, CommentRepository commentRepository)
        {
            db = context;
            commentRepo = commentRepository;
      
        }


        public IActionResult Index(int? category, int page = 1, int pageSize = 8)
        {
            var products = db.Products.AsQueryable();

            if (category.HasValue)
            {
                products = products.Where(p => p.CategoryId == category.Value);
            }

            var totalCount = products.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

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
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(result);
        }

       
        public IActionResult Search(string? query)
        {
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                // Tìm kiếm theo tên sản phẩm hoặc hãng sản phẩm
                products = products.Where(p => p.ProductName.Contains(query) || p.Category.CategoryName.Contains(query));
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

        //public IActionResult DetailProduct(int id)
        //{
        //    var data = db.Products
        //        .Include(p => p.Category)
        //        .SingleOrDefault(p => p.ProductId == id);
        //    if (data == null)
        //    {
        //        TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
        //        return Redirect("/404");
        //    }

        //    var result = new DetailProductsRepository
        //    {
        //        ProductID = data.ProductId,
        //        ProductName = data.ProductName,
        //        Price = data.Price ?? 0,
        //        Image = data.Image ?? string.Empty,
        //        Image1 = data.Image1 ?? string.Empty,
        //        Image2 = data.Image2 ?? string.Empty,
        //        Describe = data.Describe ?? string.Empty,
        //        CategoryName = data.Category.CategoryName,
        //        SoLuongTon = 10,//tính sau
        //        DiemDanhGia = 5,//check sau

        //    };
        //    return View(result);
        //}

        public IActionResult DetailProduct(int id)
        {
            var product = db.Products
                .Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }

            var viewModel = new DetailProductsRepository
            {
                ProductID = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price ?? 0,
                Image = product.Image ?? string.Empty,
                Image1 = product.Image1 ?? string.Empty,
                Image2 = product.Image2 ?? string.Empty,
                Describe = product.Describe ?? string.Empty,
                CategoryName = product.Category.CategoryName,
                SoLuongTon = 10, // Giá trị thực hiện sau khi xác định
                DiemDanhGia = 5 // Giá trị thực hiện sau khi xác định
            };

            var comments = db.Comments
                .Where(c => c.ProductId == id)
                .Include(c => c.Customer)
                .Select(c => new CommentRepository
                {
                    CustomerName = c.Customer.CustomerName,
                    CommentText = c.CommentText,
                  //  CommentDate = c.Date
                })
                .ToList();

            ViewBag.Comments = comments;

            return View(viewModel);
        }


        //[HttpPost]
        //public IActionResult AddComment(int productId, string commentText)
        //{
        //    // Lấy thông tin người dùng hiện tại

        //    var userId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == Val.CLAIM_CUSTOMERID).Value;

        //    // Tạo đối tượng Comment mới
        //    var comment = new Comment
        //    {
        //        ProductId = productId,

        //        CommentText = commentText,
        //        CustomerId = userId,
        //        //  CommentDate = DateTime.Now // Set the current date and time
        //    };

        //    // Thêm Comment vào cơ sở dữ liệu
        //    db.Comments.Add(comment);
        //    db.SaveChanges();

        //    // Chuyển hướng người dùng về trang chi tiết sản phẩm
        //    return RedirectToAction("DetailProduct", new { id = productId });
        //}




        [HttpPost]
        public IActionResult AddComment(int productId, string commentText)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "Bạn phải đăng nhập mới được bình luận.";
                return RedirectToAction("DetailProduct", new { id = productId });
            }

            // Get the current user's ID
            var userId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == Val.CLAIM_CUSTOMERID)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Message"] = "Lỗi khi lấy thông tin người dùng.";
                return RedirectToAction("DetailProduct", new { id = productId });
            }

            // Create a new Comment object
            var comment = new Comment
            {
                ProductId = productId,
                CustomerId = userId,
                CommentText = commentText,
                // CommentDate = DateTime.Now // Set the current date and time
            };

            // Add the comment to the database
            db.Comments.Add(comment);
            db.SaveChanges();

            // Redirect the user back to the product details page
            return RedirectToAction("DetailProduct", new { id = productId });
        }


    }


}


