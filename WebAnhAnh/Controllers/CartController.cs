using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebAnhAnh.Models;
using WebAnhAnh.Helpers;
using WebAnhAnh.Repository;
using Microsoft.AspNetCore.Authorization;
namespace WebAnhAnh.Controllers
{
	public class CartController : Controller
	{
		private readonly PaypalClient _paypalClient;
		private readonly ShopShopContext db;

		public CartController(ShopShopContext context, PaypalClient paypalClient)
		{
			_paypalClient = paypalClient;
			db = context;
		}

		public List<CartRepository> Cart => HttpContext.Session.Get<List<CartRepository>>(Val.CART_KEY) ?? new List<CartRepository>();

		public IActionResult Index()
		{
			return View(Cart);
		}

		public IActionResult AddToCart(int id, int quantity = 1)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.ProductID == id);
			if (item == null)
			{
				var hangHoa = db.Products.SingleOrDefault(p => p.ProductId == id);
				if (hangHoa == null)
				{
					TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
					return Redirect("/404");
				}
				item = new CartRepository
				{
					ProductID = hangHoa.ProductId,
					ProductName = hangHoa.ProductName,
					Price = hangHoa.Price ?? 0,
					Image = hangHoa.Image ?? string.Empty,
					Quantity = quantity
				};
				gioHang.Add(item);
			}
			else
			{
				item.Quantity += quantity;
			}

			HttpContext.Session.Set(Val.CART_KEY, gioHang);

			return RedirectToAction("Index");
		}

		public IActionResult RemoveCart(int id)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.ProductID == id);
			if (item != null)
			{
				gioHang.Remove(item);
				HttpContext.Session.Set(Val.CART_KEY, gioHang);
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		[HttpGet]
		public IActionResult Checkout()
		{
			if (Cart.Count == 0)
			{
				return Redirect("/");
			}

			ViewBag.PaypalClientdId = _paypalClient.ClientId;
			return View(Cart);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Checkout(CheckoutRepository model)
		{
			if (ModelState.IsValid)
			{
				var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == Val.CLAIM_CUSTOMERID).Value;
				var khachHang = new Customer();
				if (model.GiongKhachHang)
				{
					khachHang = db.Customers.SingleOrDefault(kh => kh.CustomerId == customerId);
				}

				var hoadon = new Order
				{
					CustomerId = customerId,
					CustomerName = model.CustomerName ?? khachHang.CustomerName,
					Address = model.Address ?? khachHang.Address,
					PhoneNumber = model.PhoneNumber ?? khachHang.PhoneNumber,
					OrderDate = DateTime.Now,
					HowToPay = "COD",

					StatusId = 1,
					Note = model.Note
				};


				db.Database.BeginTransaction();
				try
				{

					db.Add(hoadon);
					db.SaveChanges();

					var cthds = new List<OrderDetailId>();
					foreach (var item in Cart)
					{
						cthds.Add(new OrderDetailId
						{
							OrderId = hoadon.OrderId,
							Quantity = item.Quantity,
							Price = item.Price,
							ProductId = item.ProductID,
							Discount = 0
						});
					}
					db.AddRange(cthds);
					db.SaveChanges();
					db.Database.CommitTransaction();

					HttpContext.Session.Set<List<CartRepository>>(Val.CART_KEY, new List<CartRepository>());

					return View("Success");
				}
				catch
				{
					db.Database.RollbackTransaction();
				}
			}

			return View(Cart);
		}

		[Authorize]
		public IActionResult PaymentSuccess()
		{
			return View("Success");
		}

		#region Paypal payment
		[Authorize]
		[HttpPost("/Cart/create-paypal-order")]
		public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
		{
			// Thông tin đơn hàng gửi qua Paypal
			var tongTien = Cart.Sum(p => p.TotalPayment).ToString();
			var donViTienTe = "USD";
			var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();

			try
			{
				var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		[Authorize]
		[HttpPost("/Cart/capture-paypal-order")]
		public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _paypalClient.CaptureOrder(orderID);

				// Lưu database đơn hàng của mình

				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		#endregion
	}
}
