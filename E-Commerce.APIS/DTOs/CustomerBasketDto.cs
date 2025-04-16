using E_Commerce.Core.Entities.Basket;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.APIS.DTOs
{
    public class CustomerBasketDto
    {
       // [Required]
        public string? Id { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public string? DeliveryMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
