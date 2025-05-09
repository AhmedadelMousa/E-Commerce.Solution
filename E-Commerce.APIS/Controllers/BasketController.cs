using AutoMapper;

using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Repositories.Contract;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.APIS.Controllers
{
    [Authorize]
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basket;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BasketController(IBasketRepository basket, IMapper mapper ,IUnitOfWork unitOfWork)
        {
            _basket = basket;
            _mapper = mapper;
           _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketAsync()
        {

            var basketId = GetBasketIdFromToken();
            if (string.IsNullOrEmpty(basketId))
                return BadRequest(new ApiResponse(400, "Basket ID not found"));

            var basket = await _basket.GetBasketAsync(basketId);
            var basketDto = _mapper.Map<CustomerBasketDto>(basket ?? new CustomerBasket(basketId));
            return Ok(basketDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasketAsync(CustomerBasketDto basketDto)//To Add Or Update Basket
        {
            var basketId = GetBasketIdFromToken();
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);
            MappedBasket.Id = basketId;

            var CreateOrUpdateBasket = await _basket.UpdateBasketAsync(MappedBasket);
            return CreateOrUpdateBasket is not null ? Ok(CreateOrUpdateBasket) : BadRequest(new ApiResponse(400));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            var basketId = GetBasketIdFromToken();
            var deleted = await _basket.DeleteBasketAsync(basketId);
            return deleted ? NoContent() : NotFound(new ApiResponse(404));
        }

        [HttpPost("AddItems")]
        public async Task<ActionResult<CustomerBasket>> AddItemsToBasketAsync([FromBody] AddItemDto ItemDto)
        {
            if (ItemDto == null || string.IsNullOrEmpty(ItemDto.ProductId) || ItemDto.Quantity <= 0)
                return BadRequest(new ApiResponse(400, "Invalid product data"));
            var product = await _unitOfWork.Repository<Product>()
                                        .GetQueryable()
                                        .Include(p => p.Category)
                                        .FirstOrDefaultAsync(p => p.Id == ItemDto.ProductId);

           
            if (product == null)
                return NotFound(new ApiResponse(404, "Product not found"));
            var basketId = GetBasketIdFromToken();
            var basket = await _basket.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);
            var basketItem = new BasketItem
            {
                Id = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                Category=product.Category.Name,
                Quantity=ItemDto.Quantity,
            };

            //var basketItem = _mapper.Map<AddItemDto, BasketItem>(ItemDto);
            var existingItem = basket.Items.FirstOrDefault(i => i.Id == basketItem.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += basketItem.Quantity;
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

            var basketId = GetBasketIdFromToken();
            var basket = await _basket.GetBasketAsync(basketId);
            if (basket == null)
                return NotFound(new ApiResponse(404, "The basket is missing"));

            var item = basket.Items.FirstOrDefault(i => i.Id == productId);
            if (item == null)
                return NotFound(new ApiResponse(404, "The product is not in the cart"));

            item.Quantity -= quantity;
            if (item.Quantity <= 0)
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
