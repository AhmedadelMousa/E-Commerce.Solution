using E_Commerce.Core.Entities.Favorite;
using E_Commerce.Core.Repositories.Contract;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Repository.Contract
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        public FavoriteRepository(IDistributedCache cache)
        {
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
                SlidingExpiration = TimeSpan.FromDays(1),
            };
        }
        public async Task<bool> DeleteFavoriteAsync(string favoriteId)
        {
            var existingFavorite = await GetFavoriteAsync(favoriteId);
            if (existingFavorite == null) return false;

            await _cache.RemoveAsync(favoriteId);
            return true;
        }

        public async Task<CustomerFavorite?> GetFavoriteAsync(string favoriteId)
        {
            var favorite = await _cache.GetStringAsync(favoriteId);
            return favorite.IsNullOrEmpty() ? null : JsonSerializer.Deserialize<CustomerFavorite>(favorite);
        }

        public async Task<CustomerFavorite?> UpdateFavoriteAsync(CustomerFavorite favorite)
        {
            await _cache.SetStringAsync(favorite.Id, JsonSerializer.Serialize(favorite), _cacheOptions);
            var updatedFavorite = await GetFavoriteAsync(favorite.Id);
            return updatedFavorite;
        }
    }
}
