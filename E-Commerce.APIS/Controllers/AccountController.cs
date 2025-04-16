using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> _signIn;
        private readonly UserManager<AppUser> _user;
        private readonly IAuthService _auth;

        public AccountController(SignInManager<AppUser> signIn,UserManager<AppUser> user,IAuthService auth)
        {
            _signIn = signIn;
           _user = user;
            _auth = auth;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto login)
        {
            var user= await _user.FindByEmailAsync(login.EmailOrUserName);

            if (user == null)
            {
                user = await _user.FindByNameAsync(login.EmailOrUserName);
            }
            if (user == null) return Unauthorized(new ApiResponse(401, "Email or Username Not Found"));
            var result= await _signIn.CheckPasswordSignInAsync(user, login.Password,false);
            if (result.Succeeded is false) return Unauthorized(new ApiResponse(401, "Password is not Correct"));
            return Ok(new UserDto()
            {
               Email = user.Email,
               DisplayName= user.DisplayName,
               Token= await _auth.CreateTokenAsync(user,_user),
               AppUserId=user.Id
               
            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existUser= await _user.FindByNameAsync(register.UserName);
            var existEmail = await _user.FindByEmailAsync(register.Email);
            if (existEmail is not null) return BadRequest(new ApiResponse(400 , "Email already in use"));
            if (existUser is not null) return BadRequest(new ApiResponse(400, "Username already in use"));
            var user = new AppUser
            {
                DisplayName = $"{register.FirstName} {register.LastName}",
                Email = register.Email,
                UserName = register.UserName,
                PhoneNumber = register.PhoneNumber,

            };
            var result= await _user.CreateAsync(user,register.Password);
            if (result.Succeeded is false) return BadRequest(new ApiResponse(400, "Result Failed"));
            await _user.AddToRoleAsync(user, "User");
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _auth.CreateTokenAsync(user,_user)
            });

        }
    }
}
