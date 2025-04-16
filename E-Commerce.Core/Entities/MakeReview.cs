using E_Commerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities
{
   public class MakeReview
    {
        public string Comment { get; set; }
        public int NumberOfPoint { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //Fk
        public string ProductId { get; set; }
        public Product product { get; set; }
         public string AppUserId { get; set; }
        public AppUser appUser { get; set; }
    }
}
