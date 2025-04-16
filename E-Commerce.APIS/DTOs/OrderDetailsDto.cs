using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Order_Aggregrate;

namespace E_Commerce.APIS.DTOs
{
    public class OrderDetailsDto
    {
        public string AppUserId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public string? DeliveryMethodId { get; set; }
        public ICollection<OrderItem> Items { get; set; } 
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
    }
}
