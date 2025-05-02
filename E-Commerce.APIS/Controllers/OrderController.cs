using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Order_Aggregrate;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{

    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("BasketOrder")]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderForBasketDto dto)
        {
            var appUserId = GetAppUserIdFromToken();
            var basketId = GetBasketIdFromToken();
            if (string.IsNullOrEmpty(appUserId) || string.IsNullOrEmpty(basketId))
                return Unauthorized(new ApiResponse(401, "Invalid or missing token data"));

            return await HandleOrderCreation(
        appUserId,
        dto.DeliveryMethodId,
        dto.ShippingAddress,
        address => _orderService.CreateOrderAsync(appUserId, basketId, dto.DeliveryMethodId, address)
    );
            //var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(dto.DeliveryMethodId);
            //if (deliveryMethod == null)
            //    return BadRequest(); // أو ترجع BadRequest
            //var address = _mapper.Map<AddressDto, AddressOrder>(dto.ShippingAddress);
            //var order = await _orderService.CreateOrderAsync(dto.AppUserId, dto.BasketId, dto.DeliveryMethodId, address);
            //if (order is null) return BadRequest(new ApiResponse(400));
            //return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }
        [HttpPost("SingleOrder")]
        public async Task<ActionResult<OrderToReturnDto>> CreateSingleOrder(OrderForSingleOrderDto dto)
        {
            var appUserId = GetAppUserIdFromToken();
            if (string.IsNullOrEmpty(appUserId))
                return Unauthorized(new ApiResponse(401, "Invalid or missing token data"));

            //         return await HandleOrderCreation(
            //    dto.AppUserId,
            //    dto.DeliveryMethodId,
            //    dto.ShippingAddress,
            //    address => _orderService.CreateSingleOrderProduct(dto.AppUserId, dto.ProductId, dto.DeliveryMethodId, address)
            //);
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(dto.DeliveryMethodId);
            if (deliveryMethod == null)
                return BadRequest();
            var address = _mapper.Map<AddressDto, AddressOrder>(dto.ShippingAddress);
            var order = await _orderService.CreateSingleOrderProduct(appUserId, dto.ProductId, dto.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetAllOrders>>> GetOrdersForUserAsync()
        {
            var appUserId = GetAppUserIdFromToken();
            if (string.IsNullOrEmpty(appUserId))
                return Unauthorized(new ApiResponse(401));

            var orders= await _orderService.GetOrderForUserAsync(appUserId);
            if (orders == null)
            {
                return NotFound(new ApiResponse(404, "Order not found"));
            }
            var ordersDto = _mapper.Map<List<GetAllOrders>>(orders);

            return Ok(ordersDto);

        }

        [HttpGet("order")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderForUserAsync(string id)
        {
            var appUserId = GetAppUserIdFromToken();
            if (string.IsNullOrEmpty(appUserId))
                return Unauthorized(new ApiResponse(401));

            var order = await _orderService.GetOrderByIdForUserAsync(id, appUserId);
            if (order is null) return NotFound(new ApiResponse(404));
            var orderDetailsDto = _mapper.Map<OrderDetailsDto>(order);

           
            return Ok(orderDetailsDto);
          
        }

        private async Task<ActionResult<OrderToReturnDto>> HandleOrderCreation(
            string appUserId,
            string deliveryMethodId,
            AddressDto addressDto,
            Func<AddressOrder, Task<Order>> createOrderFunc)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod == null)
                return BadRequest(new ApiResponse(400, "Invalid Delivery Method"));

            var address = _mapper.Map<AddressDto, AddressOrder>(addressDto);

            var order = await createOrderFunc(address);

            if (order == null)
                return BadRequest(new ApiResponse(400, "Failed to create order"));

            var mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(mappedOrder);
        }
        private string GetAppUserIdFromToken()
        {
            return User.FindFirst("AppUserId")?.Value;
        }

        private string GetBasketIdFromToken()
        {
            return User.FindFirst("BasketId")?.Value;
        }
    }
}
