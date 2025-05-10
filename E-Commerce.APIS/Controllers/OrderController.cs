using AutoMapper;
using Azure;
using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Order_Aggregrate;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Service.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace E_Commerce.APIS.Controllers
{

   // [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork,IBasketRepository basketRepository)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("BasketOrder")]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid order data"));

            var appUserId = GetAppUserIdFromToken();
            var basketId = GetBasketIdFromToken();
            if (string.IsNullOrEmpty(appUserId) || string.IsNullOrEmpty(basketId))
                return Unauthorized(new ApiResponse(401, "Invalid or missing token data"));
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(dto.DeliveryMethodId);
            if (deliveryMethod == null)
                return BadRequest(new ApiResponse(400, "Delivery method not found"));
            var address = _mapper.Map<AddressDto, AddressOrder>(dto.ShippingAddress);

            return await HandleOrderCreation(
        appUserId,
        dto.DeliveryMethodId,
        dto.ShippingAddress,
       async(address) =>  {
           var order = await _orderService.CreateOrderAsync(appUserId, basketId, dto.DeliveryMethodId, address);
           if (order != null) 
           {
               
               await _basketRepository.DeleteBasketAsync(basketId);
           }
           return order;
       });
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("User/Orders")]
        public async Task<ActionResult<IReadOnlyList<GetAllOrdersDto>>> GetOrdersForUserAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var appUserId = GetAppUserIdFromToken();
            if (string.IsNullOrEmpty(appUserId))
                return Unauthorized(new ApiResponse(401));

            // Fetch all orders for the user
            var orders = await _orderService.GetOrderForUserAsync(appUserId);
            if (orders == null || !orders.Any())
            {
                return NotFound(new ApiResponse(404, "Order not found"));
            }

            // Apply pagination logic
            var totalCount = orders.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginatedOrders = orders
                .Skip((page - 1) * pageSize)  // Skip items based on page number
                .Take(pageSize)               // Take the number of items specified by pageSize
                .ToList();

            var ordersDto = _mapper.Map<List<GetAllOrdersDto>>(paginatedOrders);

            // Create response with pagination details
            var response = new PaginatedOrderResponseUserDto
            {
                Orders = ordersDto,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Ok(response);

        }
        //[HttpGet]
        //public async Task<ActionResult<GetAllOrders>> GetOrdersForAdmin()
        //{
        //    var orders= await _orderService.GetAllOrdersAsync();
        //    if (orders == null || !orders.Any())
        //    {
        //        return NotFound(new ApiResponse(404, "Order not found"));
        //    }
        //    var ordersDto = _mapper.Map<List<GetAllOrders>>(orders);
        //    return Ok(ordersDto);
        //}

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
          [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
            [HttpGet("admin/orders")]
            public async Task<ActionResult<IReadOnlyList<GetOrdersForAdminDto>>>GetOrdersForAdmin([FromQuery] int page=1, [FromQuery] int pageSize=5)
            {
                if (page < 1 || pageSize < 1)
                    return BadRequest(new ApiResponse(400, "Invalid pagination parameters"));
                var orders = await _orderService.GetAllOrdersAsync();
                if (orders == null || !orders.Any())
                    return NotFound(new ApiResponse(404, "No orders found"));

                var totalCount = orders.Count;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var paginatedOrders = orders
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                var ordersDto =  _mapper.Map<List<GetOrdersForAdminDto>>(paginatedOrders);
                var response = new PaginatedOrderResponseAdminDto
                {
                    Orders = ordersDto,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };
                return Ok(response);
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
      
    }
}
