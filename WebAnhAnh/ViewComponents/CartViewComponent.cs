using Microsoft.AspNetCore.Mvc;
using WebAnhAnh.Helpers;
using WebAnhAnh.Repository;
namespace WebAnhAnh.ViewComponents
{
	public class CartViewComponent : ViewComponent
	{
		const string CART_KEY = "MYCART";
		public IViewComponentResult Invoke()
		{
			var cart = HttpContext.Session.Get<List<CartRepository>>(CART_KEY) ?? new List<CartRepository>();
			 
			return View("CartPanel", new CartModelRepository
			{
				Quantity = cart.Sum(p => p.Quantity),
				TotalPayment = cart.Sum(p => p.TotalPayment)
			});
		}
	}
}
