using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [Route("admin/customers")]
    public class CustomersController : Controller
    {
        ShopShopContext db = new ShopShopContext();
        private readonly ShopShopContext _context;

        public CustomersController(ShopShopContext context)
        {
            _context = context;
        }
        [Route("")]
        [Route("index")]
        // GET: Admin/Customers


        public IActionResult Index(int? page)
        {
            // Lấy thông báo từ TempData (nếu có)
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var sanpham = db.Customers.AsNoTracking().OrderBy(x => x.PhoneNumber);
            PagedList<Customer> lst = new PagedList<Customer>(sanpham, pageNumber, pageSize);
            return View(lst);
        }
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Customers.ToListAsync());
        //}

        // GET: Admin/Customers/Details/5

        [Route("Edit")]
        [HttpGet]
        // GET: Admin/Customers/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(customer);
        //}
        public IActionResult Edit(string id)
        {
            var sp = db.Customers.Find(id);
            return View(sp);
        }
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer sp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index", "Customers");
            }
            return View(sp);
        }



        // POST: Admin/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //public async Task<IActionResult> Edit(string id, [Bind("CustomerId,PassWord,CustomerName,DateOfBirth,Address,PhoneNumber,Email,Effect,Role,RandomKey,IsAdmin")] Customer customer)
        //{
        //    if (id != customer.CustomerId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(customer);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CustomerExists(customer.CustomerId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("index", "customers");
        //        //return RedirectToAction(nameof(Index));
        //    }
        //    return View(customer);
        //}


        [Route("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            var category = db.Customers.Include(c => c.Orders).FirstOrDefault(c => c.CustomerId == id);

            if (category == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy danh mục
            }

            // Kiểm tra nếu danh mục này có sản phẩm
            if (category.Orders.Any())
            {
                // Nếu có, truyền thông báo vào TempData
                TempData["ErrorMessage"] = "Không thể xóa tài khoản này vì đang có đơn hàng  .";
                return RedirectToAction("index");
            }

            // Nếu không có sản phẩm nào thuộc danh mục này, thì xóa danh mục
            db.Customers.Remove(category);
            db.SaveChanges();

            return RedirectToAction("index");
        }

        //private bool CustomerExists(string id)
        //{
        //    return _context.Customers.Any(e => e.CustomerId == id);
        //}
    }
}
