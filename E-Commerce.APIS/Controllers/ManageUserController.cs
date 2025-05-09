using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.APIS.Controllers
{
  
    public class ManageUserController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUserController(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
           _roleManager = roleManager;
        }
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
                var users= await _userManager.Users.ToListAsync();
            var usersDto = new List<GetAllUsersDto>();
            foreach (var user in users)
            {
                var roles= await _userManager.GetRolesAsync(user);
                usersDto.Add(new GetAllUsersDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = roles,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });
            }
            return Ok(usersDto);
        }

        [HttpPost("UpdateUser")]
        public async Task<ActionResult> UpdateUser(string id,string?newUserName, string? newEmail, string? currentPassword, string? newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest(new ApiResponse(400, "User not found"));

            if (!string.IsNullOrWhiteSpace(newUserName))
            {
                var existingUserByName = await _userManager.FindByNameAsync(newUserName);
                if (existingUserByName != null && existingUserByName.Id != user.Id)
                    return BadRequest(new ApiResponse(400, "Username already exists"));

                user.UserName = newUserName;
            }

            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                var existingUserByEmail = await _userManager.FindByEmailAsync(newEmail);
                if (existingUserByEmail != null && existingUserByEmail.Id != user.Id)
                    return BadRequest(new ApiResponse(400, "Email already exists"));

                user.Email = newEmail;
            }
            if (!string.IsNullOrWhiteSpace(currentPassword) && !string.IsNullOrWhiteSpace(newPassword))
            {
                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!passwordChangeResult.Succeeded)
                    return BadRequest(new ApiResponse(400, "Current password is incorrect or new password is invalid"));
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User updated successfully");
        }
        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(string userId ,string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest(new ApiResponse(400, "User not found"));
            if (!await _roleManager.RoleExistsAsync(roleName)) return BadRequest(new ApiResponse(400, "Role does not exist"));
            if (await _userManager.IsInRoleAsync(user, roleName))
                return BadRequest(new ApiResponse(400, "User is already in this role"));
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok($"User {user.UserName} added to role {roleName}");
        }
        [HttpDelete("RemoveUserFromRole")]
        public async Task<ActionResult> DeleteUserFromRole(string UserId, string RoleName)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return BadRequest(new ApiResponse(400, "User not found"));
            if (!await _userManager.IsInRoleAsync(user, RoleName))
                return BadRequest(new ApiResponse(400, "User is not in this role"));
            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.UpdateSecurityStampAsync(user);

            return Ok($"User {user.UserName} removed from role {RoleName}");

        }
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null) return BadRequest(new ApiResponse(400, "User not found"));
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"User {user.UserName} deleted successfully");

        }
    }
}
