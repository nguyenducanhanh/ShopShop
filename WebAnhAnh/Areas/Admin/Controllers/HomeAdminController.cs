using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using X.PagedList;

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
            var sanpham = db.Products.Include(p => p.Category).Include(p => p.Supplier);
            PagedList<Product> lst = new PagedList<Product>(sanpham, pageNumber, pageSize);
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
                return RedirectToAction("ListProducts");
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
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListProducts", "HomeAdmin");
            }
            return View(sp);
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



