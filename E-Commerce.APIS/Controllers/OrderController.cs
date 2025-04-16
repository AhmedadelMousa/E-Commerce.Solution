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

        public OrderController(IOrderService orderService , IMapper mapper,IUnitOfWork unitOfWork )
        {
            _orderService = orderService;
            _mapper = mapper;
           _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto dto)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(dto.DeliveryMethodId);
            if (deliveryMethod == null)
                return BadRequest(); // أو ترجع BadRequest
            var address= _mapper.Map<AddressDto,AddressOrder>(dto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(dto.AppUserId,dto.BasketId,dto.DeliveryMethodId,address);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetAllOrders>>> GetOrdersForUserAsync(string AppUserId)
        {
            var orders= await _orderService.GetOrderForUserAsync(AppUserId);
            if (orders == null)
            {
                return NotFound(new ApiResponse(404, "Order not found"));
            }
            var ordersDto = _mapper.Map<List<GetAllOrders>>(orders);

            return Ok(ordersDto);

        }

        [HttpGet("order")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderForUserAsync(string id, string AppUserId)
        {
            var order = await _orderService.GetOrderByIdForUserAsync(id, AppUserId);
            if (order is null) return NotFound(new ApiResponse(404));
            var orderDetailsDto = _mapper.Map<OrderDetailsDto>(order);

            // إرجاع البيانات المحولة
            return Ok(orderDetailsDto);
          
        }
    }
}
