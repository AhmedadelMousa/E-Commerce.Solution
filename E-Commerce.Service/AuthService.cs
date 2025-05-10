using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Service.Helpers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtConfigurations _jwtConf;

        public AuthService(UserManager<AppUser> userManager, IOptions<JwtConfigurations> options)
        {
            _userManager = userManager;
            _jwtConf = options.Value;
        }
        public async Task<string> CreateTokenForUserAsync(AppUser user)
        {
            var authClaims = new List<Claim>()
               {
                   new Claim(ClaimTypes.Name,user.DisplayName),
                   new Claim(ClaimTypes.Email,user.Email),
                   //new Claim("AppUserId", user.Id)
                   new Claim(ClaimTypes.NameIdentifier, user.Id),
               };
            var userRoles= await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles) 
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            if(string.IsNullOrEmpty(user.BasketId))
            {
                user.BasketId = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);
            }

            authClaims.Add(new Claim(CustomClaimTypes.BasketId, user.BasketId));

            if(string.IsNullOrEmpty(user.FavoriteId))
            {
                user.FavoriteId = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);
            }
            authClaims.Add(new Claim(CustomClaimTypes.FavoriteId, user.FavoriteId));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConf.AuthKey));
            var Token = new JwtSecurityToken(
                audience: _jwtConf.ValidAudience,
                issuer: _jwtConf.ValidIssuer,
                expires: DateTime.Now.AddDays(_jwtConf.DurationInDays),
                claims: authClaims,

                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

    }
}
