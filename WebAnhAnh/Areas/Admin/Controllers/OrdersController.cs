using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAnhAnh.Models;
using X.PagedList;

namespace WebAnhAnh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("admin/orders")]
    public class OrdersController : Controller
    {
        private readonly ShopShopContext _context;
        ShopShopContext db = new ShopShopContext();
        public OrdersController(ShopShopContext context)
        {
            _context = context;
        }


      

        public IActionResult Index(int? page)
        {
            // Lấy thông báo từ TempData (nếu có)
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var sanpham = db.Orders.Include(p => p.Status).AsNoTracking().OrderBy(x => x.PhoneNumber);
            PagedList<Order> lst = new PagedList<Order>(sanpham, pageNumber, pageSize);
            return View(lst);
        }

        // POST: Admin/Orders/Delete/5




        [Route("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Tìm đơn hàng cần xóa từ bảng Orders
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy đơn hàng
            }

            // Tìm tất cả các mục chi tiết đơn hàng liên quan từ bảng OrderDetailId
            var orderDetails = db.OrderDetailIds.Where(od => od.OrderId == id);

            // Xóa tất cả các mục chi tiết đơn hàng
            db.OrderDetailIds.RemoveRange(orderDetails);

            // Xóa đơn hàng từ bảng Orders
            db.Orders.Remove(order);

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            return RedirectToAction("Index");
        }








        // GET: Admin/Orders/Details/5
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // Lấy các chi tiết của hóa đơn từ bảng OrderDetail
            var orderDetails = await _context.OrderDetailIds
                .Include(od => od.Product) // Nếu có các thông tin của sản phẩm bạn muốn hiển thị
                .Where(od => od.OrderId == id)
                .ToListAsync();

            ViewBag.OrderDetails = orderDetails;

            return View(order);
        }


    }
}
