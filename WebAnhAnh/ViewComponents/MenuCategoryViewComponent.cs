using Microsoft.AspNetCore.Mvc;
using WebAnhAnh.Models;
using WebAnhAnh.Repository;

namespace WebAnhAnh.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly ShopShopContext db;

        public MenuCategoryViewComponent(ShopShopContext context) => db = context;
       

      

        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(lo => new MenuCategoryRepository
            {
                CategoryId = lo.CategoryId,
                CategoryName = lo.CategoryName,
                Quantity = lo.Products.Count,
            }).OrderBy(p => p.CategoryName);

            return View(data); 
                              
        }
    }
}
