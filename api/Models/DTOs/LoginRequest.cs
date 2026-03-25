using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
