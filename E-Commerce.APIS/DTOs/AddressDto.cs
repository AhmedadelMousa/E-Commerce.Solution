using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Commerce.APIS.DTOs
{
    public class AddressDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [JsonPropertyName("Country")]
        public string Countrty { get; set; }
    }
}
