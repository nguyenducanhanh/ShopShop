using Microsoft.AspNetCore.Mvc;
using WebAnhAnh.Models;

namespace WebAnhAnh.Controllers
{
    public class AccessController : Controller
    {
        ShopShopContext db = new ShopShopContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Product");

            }
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = db.Users.Where(x => x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.UserName.ToString());
                    return RedirectToAction("Index", "Product");

                }

            }
            return View();
        }
    }
}
