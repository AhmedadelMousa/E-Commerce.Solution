using System.ComponentModel.DataAnnotations;

namespace E_Commerce.APIS.DTOs
{
    public class OrderDto
    {
        [Required]
        public string? DeliveryMethodId { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
    }
}
