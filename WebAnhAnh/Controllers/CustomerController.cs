using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using WebAnhAnh.Helpers;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ShopShopContext db;
		private readonly IMapper _mapper;
		public CustomerController(ShopShopContext context, IMapper mapper)
        {
            db = context;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
        }
		[HttpPost]
		public IActionResult Register(RegisterRepository model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var khachHang = _mapper.Map<Customer>(model);
					khachHang.RandomKey = MyUtil.GenerateRamdomKey();
					khachHang.PassWord = model.PassWord.ToMd5Hash(khachHang.RandomKey);
					khachHang.Effect = true;//sẽ xử lý khi dùng Mail để active
                    khachHang.IsAdmin = true;
                    khachHang.Role = 0;
					db.Add(khachHang);
					db.SaveChanges();
					return RedirectToAction("Index", "Product");
				}
				catch (Exception ex)
				{
					var mess = $"{ex.Message} shh";
				}
			}
			return View();
		}

        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
		[HttpPost]
        public async Task<IActionResult> Login(LoginRepository model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.Customers.SingleOrDefault(kh => kh.CustomerId == model.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Không có khách hàng này");
                }
                else
                {
                    if (!khachHang.Effect)
                    {
                        ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                    }
                    else
                    {
                        if (khachHang.PassWord != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                        }
                        else
                        {
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.Name, khachHang.CustomerName),
                                new Claim(Val.CLAIM_CUSTOMERID, khachHang.CustomerId),

        						//claim - role động
        						new Claim(ClaimTypes.Role, "Customer")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }
                    }
                }
            }
            return View();
        }

     


        [Authorize]   // chưa đăng nhập ko đc vô trang này 
		public IActionResult Profile()
		{
			return View();
		}

		[Authorize]   // chưa đăng nhập ko đc vô trang này muốn vô thì bỏ chữ  [Authorize] đi 
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}
	}
}
