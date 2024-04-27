using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using X.PagedList;
using System.Linq;

namespace WebAnhAnh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
    
        ShopShopContext db = new ShopShopContext();
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("ListProducts")]
        public IActionResult ListProducts(int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            // Get all products from the database
            var products = db.Products.Include(p => p.Category).Include(p => p.Supplier);

          

            // Paginate the filtered products
            PagedList<Product> lst = new PagedList<Product>(products, pageNumber, pageSize);
            return View(lst);
        }

        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(),
               "CategoryId", "CategoryName");
            ViewBag.SupplierId = new SelectList(db.Suppliers.ToList(),
                 "SupplierId", "CompanyName");
            return View(); 
        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(Product sanP)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(sanP);
                db.SaveChanges();
                // Sắp xếp danh sách sản phẩm theo thứ tự giảm dần của thời gian tạo
                var latestProducts = db.Products.OrderBy(p => p.DateOfManufacture).ToList();

                return RedirectToAction("ListProducts", latestProducts);
            }
            return View(sanP);
        }
        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(int id)
        {
        
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(),
               "CategoryId", "CategoryName");
            ViewBag.SupplierId = new SelectList(db.Suppliers.ToList(),
                 "SupplierId", "CompanyName");
            var sp = db.Products.Find(id);
           
            return View(sp);
        }
        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(Product sp)
        {
            if (ModelState.IsValid)
            {
                // Truy xuất thông tin sản phẩm từ cơ sở dữ liệu
                var existingProduct = db.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == sp.ProductId);

                // Kiểm tra nếu người dùng không chọn ảnh mới, sử dụng ảnh hiện có
                if (sp.Image == null)
                {
                    sp.Image = existingProduct.Image;
                    sp.Image1 = existingProduct.Image1;
                    sp.Image2 = existingProduct.Image2;
                }

                // Kiểm tra nếu người dùng chọn ảnh mới, sử dụng ảnh mới


                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListProducts", "HomeAdmin");
            }
            return View(sp);
        }
        private readonly IWebHostEnvironment _webHostEnvironment;

     
        private string SaveImage(IFormFile image)
        {
            // Thư mục lưu trữ ảnh
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            // Tạo tên tệp tin duy nhất để tránh xung đột tên tệp tin
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

            // Đường dẫn đầy đủ đến tệp tin
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu ảnh vào thư mục
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            // Trả về đường dẫn của ảnh
            return "/images/" + uniqueFileName; // Đường dẫn này phải phản ánh cấu trúc thư mục trong wwwroot
        }

        [Route("XoaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaSanPham(int id)
        {
            var sanPham = db.Products.Find(id);
            if (sanPham == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy sản phẩm
            }

            db.Products.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("ListProducts");
        }

    }
}



