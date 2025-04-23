using E_Commerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities
{
   public class Review : BaseEntity
    {
        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }
        [Range(0, 5)]
        [Required]
        public int NumberOfPoint { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //Fk
        public string ProductId { get; set; }
        public Product Product { get; set; }
         public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
