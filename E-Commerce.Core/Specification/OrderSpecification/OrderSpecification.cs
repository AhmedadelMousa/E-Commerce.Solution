using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Order_Aggregrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specification.OrderSpecification
{
    public class OrderSpecification:BaseSpecification<Order>
    {
        public OrderSpecification(string orderId,string appUserId) :base(o=>o.Id==orderId &&o.AppUserId== appUserId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o=>o.Items);
        }
        public OrderSpecification(string appUserId) :base(o=>o.AppUserId== appUserId)
        {
            Includes.Add(o=>o.Items);
            Includes.Add(o => o.DeliveryMethod);
           
        }
    }
}
