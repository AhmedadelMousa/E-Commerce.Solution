using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.APIS.Controllers
{
  
    public class RoleController : BaseApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<ActionResult> AddRole(AddRoleDto  roleDto)
        {
            if (string.IsNullOrWhiteSpace(roleDto.RoleName)) return BadRequest(new ApiResponse(400, "Role name cannot be empty"));
            var roleExists= await _roleManager.RoleExistsAsync(roleDto.RoleName);
            if (roleExists) return BadRequest(new ApiResponse(400, "Role already exists"));
            var Result= await _roleManager.CreateAsync(new IdentityRole(roleDto.RoleName));
            if (!Result.Succeeded) return BadRequest(Result.Errors);
            return Ok($"Role {roleDto.RoleName} Created Successfully");

        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return Ok("Role Deleted Sucssefully");
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();

            return Ok(roles);
        }
    }
}
