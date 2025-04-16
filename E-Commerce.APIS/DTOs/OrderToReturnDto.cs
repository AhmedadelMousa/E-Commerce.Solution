using E_Commerce.Core.Order_Aggregrate;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.APIS.DTOs
{
    public class OrderToReturnDto
    {
        public string Id { get; set; }//Num of order are created
        public string AppUserId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public AddressOrder ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
