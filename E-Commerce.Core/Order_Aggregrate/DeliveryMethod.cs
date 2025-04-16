using E_Commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Order_Aggregrate
{
    public class DeliveryMethod:BaseEntity
    {
        public DeliveryMethod(string shortName,string description,string deliveryTime,decimal cost)
        {
            ShortName = shortName;
            Description = description;
            DeliveryTime = deliveryTime;
            Cost = cost;
        }
     
        public DeliveryMethod()
        {
            
        }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public Decimal Cost { get; set; }
    }
}
