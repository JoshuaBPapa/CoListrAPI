using System.ComponentModel.DataAnnotations;

namespace CoListrAPI.DTOs.Auth
{
    public class AuthLoginDto
    {
        [Required]
        [MaxLength(255)]
        [MinLength(3)]
        [RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessage = "Only letters, numbers, _ and - are allowed.")]
        public required string Username { get; set; }
        [Required]
        [MaxLength(255)]
        [MinLength(8)]
        public required string Password { get; set; }
    }
}
