using Microsoft.AspNetCore.Mvc;

namespace WebAnhAnh.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
