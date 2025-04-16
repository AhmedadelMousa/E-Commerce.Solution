using System.ComponentModel.DataAnnotations;

namespace E_Commerce.APIS.DTOs
{
    public class AddRoleDto
    {
        [Required(ErrorMessage = "Name Is Required")]
        public string RoleName { get; set; }
    }
}
