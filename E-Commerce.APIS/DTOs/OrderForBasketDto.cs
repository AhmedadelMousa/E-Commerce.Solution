using System.ComponentModel.DataAnnotations;

namespace E_Commerce.APIS.DTOs
{
    public class OrderForBasketDto:OrderDto
    {
        [Required]
        public string BasketId { get; set; }
    }
}
