using E_Commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Order_Aggregrate
{
    public class OrderItem:BaseEntity
    {
        public OrderItem(ProductItemOrdered product,decimal price,int quantity=1)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }
        public OrderItem()
        {
            
        }
        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }   
    }
}
