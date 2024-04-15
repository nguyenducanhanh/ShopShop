using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAnhAnh.Repository
{
    public class ProductsRepository
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public double Price { get; set; }
        public string Describe { get; set; }
        public string CategoryName { get; set; }
 
    }

    public class DetailProductsRepository
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public double Price { get; set; }
        public string Describe { get; set; }
        public string CategoryName { get; set; }
      
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }
    }
}
