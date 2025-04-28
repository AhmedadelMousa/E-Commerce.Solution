using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities.Favorite
{
    public class CustomerFavorite
    {
        public CustomerFavorite(string id)
        {
            Id = id;
            Items = new List<FavoriteItem>();
        }
        public string Id { get; set; } 
       public List<FavoriteItem> Items { get; set; }
    }
}
