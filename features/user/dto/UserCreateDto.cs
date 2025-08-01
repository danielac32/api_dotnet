 
using System.ComponentModel.DataAnnotations;

namespace backend_ont_2.features.user.dto
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; }

        [Required]
        public string Position { get; set; }
    }
}