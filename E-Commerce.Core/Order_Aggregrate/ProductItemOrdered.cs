using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Order_Aggregrate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered(string productId,string productName,string productUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ProductUrl = productUrl;
        }
        public ProductItemOrdered()
        {
            
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
    }
}
