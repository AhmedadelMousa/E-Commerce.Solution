using System.ComponentModel.DataAnnotations;

namespace E_Commerce.APIS.DTOs
{
    public class OrderDto
    {
        [Required]
        public string AppUserId { get; set; }
       
        [Required]
        public string? DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
