using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAnhAnh.Controllers;
using WebAnhAnh.Models;
using WebAnhAnh.Services;

namespace WebAnhAnh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    //[Route("admin")]
    [Route("admin/statistical")]
    public class StatisticalController : Controller
    {
        private readonly ShopShopContext _db;
        private readonly IMapper _mapper;
        private readonly CustomerService _customerService;

        public StatisticalController(ShopShopContext context, IMapper mapper, CustomerService customerService)
        {
            _db = context;
            _mapper = mapper;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            int registeredAccountsCount = _customerService.GetRegisteredAccountsCount();
            ViewBag.RegisteredAccountsCount = registeredAccountsCount;
            int od = _customerService.GetOrdersCount();
            ViewBag.OrdersCount = od;
            int oddt = _customerService.GetOrderDetailIdsCount();
            ViewBag.GetOrderDetailIdsCount = oddt;
            double oddtt = _customerService.GetTotalMoneySum();
            ViewBag.GetTotalMoneySum = oddtt;
            return View();
        }

        
    }

}


