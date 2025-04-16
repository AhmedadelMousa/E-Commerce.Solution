using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Order_Aggregrate
{
    public class Order :BaseEntity
    {
        public Order(string appUserId, AddressOrder shippingAddress,DeliveryMethod deliveryMethod,ICollection<OrderItem> items ,decimal subTotal)
        {
            AppUserId=appUserId;
            ShippingAddress=shippingAddress;
            DeliveryMethod=deliveryMethod;
            Items=items;
            SubTotal=subTotal;
        }
        public Order()
        {
            
        }
        public string AppUserId { get; set; }
        public AppUser  User { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status   { get; set; }
        public AddressOrder ShippingAddress { get; set; }
        public string? DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }//=OrderItem*Quantity
        [NotMapped]
        public decimal Total => SubTotal + DeliveryMethod.Cost;
        //public string? PaymentIntentId { get; set; } = string.Empty;
        public PaymentMethod paymentMethod { get; set; }
    }
}
