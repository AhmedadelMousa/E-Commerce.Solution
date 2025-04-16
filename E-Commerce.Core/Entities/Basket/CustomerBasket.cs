using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities.Basket
{
    public enum PaymentMethod
    {
        CashOnDelivery,  // الدفع عند الاستلام
        OnlinePayment    // الدفع الإلكتروني
    }
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItem>();
        }
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
        public string? DeliveryMethodId { get; set; }
        public PaymentMethod  PaymentMethod { get; set; }
    }
}
