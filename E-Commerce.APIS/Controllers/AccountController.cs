using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Enums;
using E_Commerce.Core.Services.Contract;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> _signIn;
        private readonly UserManager<AppUser> _user;
        private readonly IAuthService _auth;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signIn, UserManager<AppUser> user, IAuthService auth, RoleManager<IdentityRole> roleManager)
        {
            _signIn = signIn;
            _user = user;
            _auth = auth;
            _roleManager = roleManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            try
            {
                var user = await _user.FindByEmailAsync(login.EmailOrUserName);

                if (user == null)
                {
                    user = await _user.FindByNameAsync(login.EmailOrUserName);
                }
                if (user == null)
                    return Unauthorized(new ApiResponse(401, "Email or Username Not Found"));

                var result = await _signIn.CheckPasswordSignInAsync(user, login.Password, false);
                if (result.Succeeded is false)
                    return Unauthorized(new ApiResponse(401, "Password is not Correct"));

                return Ok(new UserDto()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await _auth.CreateTokenAsync(user, _user),
                    AppUserId = user.Id,
                    Role = user.Role.ToString()
                });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog or NLog)
                return StatusCode(500, new ApiResponse(500, $"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existUser = await _user.FindByNameAsync(register.UserName);
                var existEmail = await _user.FindByEmailAsync(register.Email);

                if (existEmail is not null)
                    return BadRequest(new ApiResponse(400, "Email already in use"));
                if (existUser is not null)
                    return BadRequest(new ApiResponse(400, "Username already in use"));

                var user = new AppUser
                {
                    DisplayName = register.UserName,
                    Email = register.Email,
                    UserName = register.Email,
                    Role = AppRole.User,
                    FavoriteId=Guid.NewGuid().ToString(),
                    BasketId=Guid.NewGuid().ToString(),
                };

                var result = await _user.CreateAsync(user, register.Password);
                if (result.Succeeded is false)
                    return BadRequest(new ApiResponse(400, "Result Failed"));

                if (!await _roleManager.RoleExistsAsync(AppRole.User.ToString()))
                    await _roleManager.CreateAsync(new()
                    {
                        Name = AppRole.User.ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedName = AppRole.User.ToString().ToUpper(),
                        Id = Guid.NewGuid().ToString()
                    });

                var roleResult = await _user.AddToRoleAsync(user, AppRole.User.ToString());
                if (roleResult.Succeeded is false)
                    return BadRequest(new ApiResponse(400, "Role Result Failed"));

                return Ok(new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await _auth.CreateTokenAsync(user, _user),
                    Role = user.Role.ToString(),
                    AppUserId = user.Id
                });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog or NLog)
                return StatusCode(500, new ApiResponse(500, $"An error occurred: {ex.Message}"));
            }
        }
    }
}
