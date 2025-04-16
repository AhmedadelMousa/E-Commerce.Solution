using E_Commerce.Core.Entities.Basket;
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
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
                SlidingExpiration = TimeSpan.FromDays(1),
            };
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var existingBasket = await GetBasketAsync(basketId);
            if (existingBasket == null) return false; 

            await _cache.RemoveAsync(basketId);
            return true; 
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
           var basket= await _cache.GetStringAsync(basketId);
            return basket.IsNullOrEmpty() ? null : JsonSerializer.Deserialize<CustomerBasket>(basket); 
        }
        //Create Or Update
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
             await _cache.SetStringAsync(basket.Id, JsonSerializer.Serialize(basket));
            var updatedBasket = await GetBasketAsync(basket.Id);
            if (updatedBasket == null) return null;
            return updatedBasket;
        }
    }
}
