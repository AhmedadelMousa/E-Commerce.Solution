using E_Commerce.Core.Entities.Favorite;

namespace E_Commerce.APIS.DTOs
{
    public class CustomerFavoriteDto
    {
        public string? Id { get; set; }
        public List<FavoriteItem> Items { get; set; }
    }
}
