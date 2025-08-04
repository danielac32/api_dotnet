 
using System.ComponentModel.DataAnnotations;

namespace backend_ont_2.features.user.dto
{

    public class LoginDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(50, ErrorMessage = "El correo no puede exceder 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatorio.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }



    public class UserCreateDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(50, ErrorMessage = "El correo no puede exceder 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatorio.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MinLength(6, ErrorMessage = "El nombre debe tener al menos 6 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El departamento es obligatorio.")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio.")]
        public string Position { get; set; }

        public string? ProfileImage { get; set; }
    }


    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
        public string? Token { get; set; }
    }
    
    public class UserDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }
        public bool? IsActive { get; set; }
        public string? Position { get; set; }
        public string? ProfileImage { get; set; }
    }

}