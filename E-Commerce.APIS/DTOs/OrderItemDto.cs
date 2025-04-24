using E_Commerce.Core.Order_Aggregrate;

namespace E_Commerce.APIS.DTOs
{
    public class OrderItemDto
    {
        public string Id { get; set; }
        //public string ProductId { get; set; }
        //public string ProductName { get; set; }
        //public string ProductUrl { get; set; }
        //public int Quantity { get; set; }
        public ProductItemOrderedDto Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
