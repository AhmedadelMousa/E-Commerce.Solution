using E_Commerce.Service.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace E_Commerce.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {

        protected string GetAppUserIdFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        protected string GetFavoriteIdFromToken()
        {
            return User.FindFirst(CustomClaimTypes.FavoriteId)?.Value ?? string.Empty;
        }

        protected string GetBasketIdFromToken()
        {
            return User.FindFirst(CustomClaimTypes.BasketId)?.Value ?? string.Empty;
        }
    }
}
