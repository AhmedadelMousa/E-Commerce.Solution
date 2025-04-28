using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities.Favorite;
using E_Commerce.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{
   
    public class FavoriteController : BaseApiController
    {
        private readonly IFavoriteRepository _favorite;
        private readonly IMapper _mapper;

        public FavoriteController(IFavoriteRepository favorite, IMapper mapper)
        {
            _favorite = favorite;
            _mapper = mapper;
        }

        private string GetFavoriteId()
        {
            var favoriteId = User.FindFirst("FavoriteId")?.Value; 
            if (string.IsNullOrEmpty(favoriteId))
                throw new UnauthorizedAccessException("Favorite ID not found in token");
            return favoriteId;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerFavorite>> GetFavoriteAsync()
        {
            var favoriteId = GetFavoriteId();
            var favorite = await _favorite.GetFavoriteAsync(favoriteId);
            return Ok(favorite ?? new CustomerFavorite(favoriteId));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerFavorite>> CreateOrUpdateFavoriteAsync(CustomerFavoriteDto favoriteDto)
        {
            var favoriteId = GetFavoriteId();
            var mappedFavorite = _mapper.Map<CustomerFavoriteDto, CustomerFavorite>(favoriteDto);
            mappedFavorite.Id = favoriteId;

            var createOrUpdateFavorite = await _favorite.UpdateFavoriteAsync(mappedFavorite);
            return createOrUpdateFavorite is not null ? Ok(createOrUpdateFavorite) : BadRequest(new ApiResponse(400));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            var favoriteId = GetFavoriteId();
            var deleted = await _favorite.DeleteFavoriteAsync(favoriteId);
            return deleted ? NoContent() : NotFound(new ApiResponse(404));
        }

        [HttpPost("AddItems")]
        public async Task<ActionResult<CustomerFavorite>> AddItemsToFavoriteAsync([FromBody] AddItemDto itemDto)
        {
            if (itemDto == null || string.IsNullOrEmpty(itemDto.ProductId))
                return BadRequest(new ApiResponse(400, "Invalid product data"));

            var favoriteId = GetFavoriteId();
            var favorite = await _favorite.GetFavoriteAsync(favoriteId) ?? new CustomerFavorite(favoriteId);
            var favoriteItem = _mapper.Map<AddItemDto, FavoriteItem>(itemDto);

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

            var favoriteId = GetFavoriteId();
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
