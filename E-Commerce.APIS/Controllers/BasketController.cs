using AutoMapper;

using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Repositories.Contract;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basket;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basket, IMapper mapper)
        {
            _basket = basket;
            _mapper = mapper;
        }
        private string GetBasketId()
        {
            var basketId = User.FindFirst("BasketId")?.Value;
            if (string.IsNullOrEmpty(basketId))
                throw new UnauthorizedAccessException("Basket ID not found in token");
            return basketId;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketAsync()
        {

            var basketId = GetBasketId();
            if (string.IsNullOrEmpty(basketId))
                return BadRequest(new ApiResponse(400, "Basket ID not found"));

            var basket = await _basket.GetBasketAsync(basketId);
            var basketDto = _mapper.Map<CustomerBasketDto>(basket ?? new CustomerBasket(basketId));
            return Ok(basketDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasketAsync(CustomerBasketDto basketDto)//To Add Or Update Basket
        {
            var basketId = GetBasketId();
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);
            MappedBasket.Id = basketId;

            var CreateOrUpdateBasket = await _basket.UpdateBasketAsync(MappedBasket);
            return CreateOrUpdateBasket is not null ? Ok(CreateOrUpdateBasket) : BadRequest(new ApiResponse(400));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            var basketId = GetBasketId();
            var deleted = await _basket.DeleteBasketAsync(basketId);
            return deleted ? NoContent() : NotFound(new ApiResponse(404));
        }

        [HttpPost("AddItems")]
        public async Task<ActionResult<CustomerBasket>> AddItemsToBasketAsync([FromBody] AddItemDto ItemDto)
        {
            if (ItemDto == null || string.IsNullOrEmpty(ItemDto.ProductId) || ItemDto.Quantity <= 0)
                return BadRequest(new ApiResponse(400, "Invalid product data"));
            var basketId = GetBasketId();
            var basket = await _basket.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);
            var basketItem = _mapper.Map<AddItemDto, BasketItem>(ItemDto);
            var existingItem = basket.Items.FirstOrDefault(i => i.Id == basketItem.Id);
            if (existingItem != null)
            {
                existingItem.Quentity += basketItem.Quentity;
            }
            else
            {
                basket.Items.Add(basketItem);
            }
            var updatedBasket = await _basket.UpdateBasketAsync(basket);
            if (updatedBasket == null)
                return BadRequest(new ApiResponse(400, "Cart update failed"));

            return Ok(updatedBasket);

        }
        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<CustomerBasket>> RemoveItemFromBasketAsync(string productId, [FromQuery] int quantity = 1)
        {
            if (string.IsNullOrEmpty(productId) || quantity <= 0)
                return BadRequest(new ApiResponse(400, "Invalid product ID or quantity"));

            var basketId = GetBasketId();
            var basket = await _basket.GetBasketAsync(basketId);
            if (basket == null)
                return NotFound(new ApiResponse(404, "The basket is missing"));

            var item = basket.Items.FirstOrDefault(i => i.Id == productId);
            if (item == null)
                return NotFound(new ApiResponse(404, "The product is not in the cart"));

            item.Quentity -= quantity;
            if (item.Quentity <= 0)
            {
                basket.Items.Remove(item);
            }

            var updatedBasket = await _basket.UpdateBasketAsync(basket);
            if (updatedBasket == null)
                return BadRequest(new ApiResponse(400, "Cart update failed"));

            return Ok(updatedBasket);
        }

    }
}
