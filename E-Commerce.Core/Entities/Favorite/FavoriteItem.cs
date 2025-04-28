using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities.Favorite
{
    public class FavoriteItem
    {
        public string Id { get; set; } 
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
