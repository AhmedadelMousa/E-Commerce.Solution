using System.ComponentModel.DataAnnotations;

namespace E_Commerce.APIS.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be Greater Than Zero")] 
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must be at Least one!")]
        public int Quantity { get; set; }
    }
}
