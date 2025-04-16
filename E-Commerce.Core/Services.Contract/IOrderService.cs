using E_Commerce.Core.Order_Aggregrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string appUserId, string basketId, string deliveryMethodId, AddressOrder shippingAddress);
        Task<IReadOnlyList<Order>> GetOrderForUserAsync(string appUserId);
        Task<Order?> GetOrderByIdForUserAsync(string orderId, string appUserId);
    }
}
