using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
