using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace WebAnhAnh.Areas.Admin.Controllers
{

    [Area("admin")]
    //[Route("admin")]
    [Route("admin/categories")]
    public class CategoriesController : Controller
    {

        ShopShopContext db = new ShopShopContext();
        [Route("")]
        [Route("list")]
        public IActionResult List(int? page)
        {
            // Lấy thông báo từ TempData (nếu có)
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
            int pageSize = 5;
       int pageNumber = page == null || page < 0 ? 1 : page.Value;
        var sanpham = db.Categories.AsNoTracking().OrderBy(x => x.CategoryName);
       PagedList<Category> lst = new PagedList<Category>(sanpham, pageNumber, pageSize);
        return View(lst);
   }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category sanP)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(sanP);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(sanP);
        }


        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sp = db.Categories.Find(id);
            return View(sp);
        }
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category sp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List", "Categories");
            }
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var category = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy danh mục
            }

            // Kiểm tra nếu danh mục này có sản phẩm
            if (category.Products.Any())
            {
                // Nếu có, truyền thông báo vào TempData
                TempData["ErrorMessage"] = "Không thể xóa category có chứa product.";
                return RedirectToAction("List");
            }

            // Nếu không có sản phẩm nào thuộc danh mục này, thì xóa danh mục
            db.Categories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("List");
        }


    }
}
