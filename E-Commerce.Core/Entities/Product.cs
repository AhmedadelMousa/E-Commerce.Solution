using E_Commerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string Size { get; set; }
        public string Colors { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public Category category { get; set; }
        public ICollection<MakeReview> MakeReviews { get; set; }
        
        public ICollection<AppUser>  users { get; set; }
    }
}
