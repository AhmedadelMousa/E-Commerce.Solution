
using E_Commerce.Core.Enums;

using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Address Address { get; set; }
        public AppRole Role { get; set; }
        public string BasketId { get; set; }= Guid.NewGuid().ToString();
        public string FavoriteId { get; set; } = Guid.NewGuid().ToString();
    }
}
