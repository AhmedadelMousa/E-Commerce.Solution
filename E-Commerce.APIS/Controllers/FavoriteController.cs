using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.APIS.Helpers;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Favorite;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Repository.Repository.Contract;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Commerce.APIS.Controllers
{
    [Authorize]
    public class FavoriteController : BaseApiController
    {
        private readonly IFavoriteRepository _favorite;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppBaseLinks _appLinks;

        public FavoriteController(IFavoriteRepository favorite, IMapper mapper, IUnitOfWork unitOfWork, IOptions<AppBaseLinks> options)
        {
            _favorite = favorite;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _appLinks = options.Value;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerFavorite>> GetFavoriteAsync()
        {
            var favoriteId = GetFavoriteIdFromToken();
            var favorite = await _favorite.GetFavoriteAsync(favoriteId);
            return Ok(favorite ?? new CustomerFavorite(favoriteId));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerFavorite>> CreateOrUpdateFavoriteAsync(CustomerFavoriteDto favoriteDto)
        {
            var favoriteId = GetFavoriteIdFromToken();
            if (string.IsNullOrEmpty(favoriteId))
                return Unauthorized(new ApiResponse(401, "Bad Token Request"));
            var mappedFavorite = _mapper.Map<CustomerFavoriteDto, CustomerFavorite>(favoriteDto);
            mappedFavorite.Id = favoriteId;

            var createOrUpdateFavorite = await _favorite.UpdateFavoriteAsync(mappedFavorite);
            return createOrUpdateFavorite is not null ? Ok(createOrUpdateFavorite) : BadRequest(new ApiResponse(400));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            var favoriteId = GetFavoriteIdFromToken();
            if (string.IsNullOrEmpty(favoriteId))
                return Unauthorized(new ApiResponse(401, "Bad Token Request"));
            var deleted = await _favorite.DeleteFavoriteAsync(favoriteId);
            return deleted ? NoContent() : NotFound(new ApiResponse(404));
        }

        [HttpPost("AddItems")]
        public async Task<ActionResult<CustomerFavorite>> AddItemsToFavoriteAsync([FromBody] AddItemDto itemDto)
        {
            var user = User.Claims;
            if (itemDto == null || string.IsNullOrEmpty(itemDto.ProductId))
                return BadRequest(new ApiResponse(400, "Invalid product data"));

            var favoriteId = GetFavoriteIdFromToken();
            if (string.IsNullOrEmpty(favoriteId))
                return Unauthorized(new ApiResponse(401, "Bad Token Request"));
            var product = await _unitOfWork.Repository<Product>()
                   .GetQueryable()
                   .Include(p => p.Category)
                   .FirstOrDefaultAsync(p => p.Id == itemDto.ProductId);
            if (product == null)
                return NotFound(new ApiResponse(404, "Product not found"));

            var favorite = await _favorite.GetFavoriteAsync(favoriteId) ?? new CustomerFavorite(favoriteId);
            var favoriteItem = new FavoriteItem
            {
                Category = product.Category.Name,
                PictureUrl = _appLinks.ApiBaseUrl + product.PictureUrl,
                Price = product.Price,
                ProductName = product.Name,
                Id = product.Id
            };


            var existingItem = favorite.Items.FirstOrDefault(i => i.Id == favoriteItem.Id);
            if (existingItem == null)
            {
                favorite.Items.Add(favoriteItem);
            }

            var updatedFavorite = await _favorite.UpdateFavoriteAsync(favorite);
            if (updatedFavorite == null)
                return BadRequest(new ApiResponse(400, "Favorite update failed"));

            return Ok(updatedFavorite);
        }

        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<CustomerFavorite>> RemoveItemFromFavoriteAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest(new ApiResponse(400, "Invalid product ID"));

            var favoriteId = GetFavoriteIdFromToken();
            if (string.IsNullOrEmpty(favoriteId))
                return Unauthorized(new ApiResponse(401, "Bad Token Request"));
            var favorite = await _favorite.GetFavoriteAsync(favoriteId);
            if (favorite == null)
                return NotFound(new ApiResponse(404, "The favorite list is missing"));

            var item = favorite.Items.FirstOrDefault(i => i.Id == productId);
            if (item == null)
                return NotFound(new ApiResponse(404, "The product is not in the favorite list"));

            favorite.Items.Remove(item);

            var updatedFavorite = await _favorite.UpdateFavoriteAsync(favorite);
            if (updatedFavorite == null)
                return BadRequest(new ApiResponse(400, "Favorite update failed"));

            return Ok(updatedFavorite);
        }
    }
}
