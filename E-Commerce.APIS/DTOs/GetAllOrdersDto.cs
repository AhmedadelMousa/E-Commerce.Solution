using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Order_Aggregrate;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.APIS.DTOs
{
    public class GetAllOrdersDto
    {

        public string OrderId { get; set; }
        public string Status { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderCreatedAt { get; set; }
        public int NumberOfProductsItems { get; set; }
        public string PaymentMethod { get; set; }
    }
}
