using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Entities;

namespace E_Commerce.APIS.DTOs
{
    public class ProductsDto
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
      
        public string PictureUrl { get; set; }
        public string Size { get; set; }
        
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
       

       
    }
}
