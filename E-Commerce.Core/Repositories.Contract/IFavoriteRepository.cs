using E_Commerce.Core.Entities.Favorite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Repositories.Contract
{
    public interface IFavoriteRepository
    {
        Task<CustomerFavorite?> GetFavoriteAsync(string favoriteId);
        Task<CustomerFavorite?> UpdateFavoriteAsync(CustomerFavorite favorite);
        Task<bool> DeleteFavoriteAsync(string favoriteId);
    }
}
