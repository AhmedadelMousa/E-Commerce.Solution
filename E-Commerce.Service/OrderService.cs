using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Order_Aggregrate;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Core.Specification.OrderSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
           _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order?> CreateOrderAsync(string appUserId, string basketId, string deliveryMethodId, AddressOrder shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket?.Items == null || !basket.Items.Any()) return null;

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var orderItem = await PrepareOrderItemAsync(item.Id, item.Quantity);
                if (orderItem != null)
                    orderItems.Add(orderItem);
            }

            return await CreateOrderInternalAsync(appUserId, deliveryMethodId, shippingAddress, orderItems);
            //var basket= await _basketRepository.GetBasketAsync(basketId);


            // var orderItems = new List<OrderItem>();
            // if(basket?.Items?.Count>0)
            // {
            //     foreach(var item in  basket.Items)
            //     {
            //         var product= await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            //         var productItemOrdered=new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);
            //         var orderItem=  new OrderItem(productItemOrdered,product.Price,item.Quentity);
            //         orderItem.Id= Guid.NewGuid().ToString();
            //         orderItems.Add(orderItem);
            //     }
            // }

            // var subTotal=orderItems.Sum(orderItem=> orderItem.Price* orderItem.Quantity);

            // var deliveryMethod= await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // var order = new Order(appUserId, shippingAddress, deliveryMethod, orderItems, subTotal);
            // order.paymentMethod=PaymentMethod.CashOnDelivery;
            // order.Id = Guid.NewGuid().ToString();
            // await _unitOfWork.Repository<Order>().AddAsync(order);

            // var result= await _unitOfWork.CompleteAsync();
            // if(result<=0) return null;
            // return order;
        }

        public async Task<Order?> CreateSingleOrderProduct(string appUserId, string productId, string deliveryMethodId, AddressOrder shippingAddress)
        {
            //var product= await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            //  if (product == null) return null;

            //  var productItemOrdered= new ProductItemOrdered(product.Id,product.Name,product.PictureUrl);
            //  var orderItem = new OrderItem(productItemOrdered,product.Price);
            //  orderItem.Id = Guid.NewGuid().ToString();
            //  var subTotal= orderItem.Price;
            //  var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            //  var orderItems = new List<OrderItem> { orderItem };
            //  var order= new Order(appUserId,shippingAddress,deliveryMethod,orderItems,subTotal);
            //  order.paymentMethod=PaymentMethod.CashOnDelivery;
            //  order.Id = Guid.NewGuid().ToString();
            //  await _unitOfWork.Repository<Order>().AddAsync(order);
            //  var result = await _unitOfWork.CompleteAsync();
            //  if (result <= 0) return null;
            //  return order;
            var orderItem = await PrepareOrderItemAsync(productId, 1);
            if (orderItem == null) return null;
            var orderItems = new List<OrderItem> { orderItem };
            return await CreateOrderInternalAsync(appUserId, deliveryMethodId, shippingAddress, orderItems);

        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            var orders= await _unitOfWork.Repository<Order>().GetAllAsync();
            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string orderId, string appUserId)
        {
           var OrderRepo=  _unitOfWork.Repository<Order>();
            var spec= new OrderSpecification(orderId, appUserId);
            var order= await OrderRepo.GetWithSpecAsync(spec);
            return order;

        }

        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string appUserId)
        {
           var OrderRepo= _unitOfWork.Repository<Order>();
            var spec= new OrderSpecification(appUserId);
            var orders= await OrderRepo.GetAllWithSpecAsync(spec);
            return orders;

        }

        private async Task<Order?> CreateOrderInternalAsync(string appUserId, string deliveryMethodId, AddressOrder shippingAddress,List<OrderItem> orderItems)
        {
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod == null) return null;
            var order = new Order(appUserId, shippingAddress, deliveryMethod, orderItems, subTotal)
            {
                Id = Guid.NewGuid().ToString(),
                paymentMethod = PaymentMethod.CashOnDelivery
            };
            await _unitOfWork.Repository<Order>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();

            return result <= 0 ? null : order;
        }
        private async Task<OrderItem?> PrepareOrderItemAsync(string productId, int quantity)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            if (product == null) return null;

            var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
            var orderItem = new OrderItem(productItemOrdered, product.Price, quantity)
            {
                Id = Guid.NewGuid().ToString()
            };

            return orderItem;
        }

     
    }
}
