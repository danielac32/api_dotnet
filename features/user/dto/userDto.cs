 
using System.ComponentModel.DataAnnotations;

namespace backend_ont_2.features.user.dto
{

    public class LoginDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(50, ErrorMessage = "El correo no puede exceder 50 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatorio.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }



    public class UserCreateDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(50, ErrorMessage = "El correo no puede exceder 50 caracteres.")]
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

        public bool? IsActive { get; set; }
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

    public class PermissionDto
    {
        [Required(ErrorMessage = "La sección es obligatoria")]
        [StringLength(50, ErrorMessage = "La sección no puede exceder 50 caracteres")]
        public string Section { get; set; } = string.Empty;

        public bool CanCreate { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanPublish { get; set; } = false;
    }


    public class UpdatePermissionDto
    {
        public string? Section { get; set; }
        public bool? CanCreate { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanPublish { get; set; }
    }





    public class OrganismoGobernacionCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valor1 es obligatorio")]
        public int Valor1 { get; set; }

        [Required(ErrorMessage = "Valor2 es obligatorio")]
        public int Valor2 { get; set; }

        [Required(ErrorMessage = "Valor3 es obligatorio")]
        public int Valor3 { get; set; }
    }


    public class OrganismoGobernacionUpdateDto
    {
        public string? Nombre { get; set; }

        public int? Valor1 { get; set; }
        public int? Valor2 { get; set; }
        public int? Valor3 { get; set; }
    }

    public class ResumenGestionCreateDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL de la imagen es obligatoria")]
        public string ImagenUrl { get; set; } = string.Empty;
    }

    public class ResumenGestionUpdateDto
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
    }

 
    public class NoticiaCreateDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contenido es obligatorio")]
        public string Contenido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La imagen es obligatorio")]
        public string? ImagenUrl { get; set; }
    }

    public class NoticiaUpdateDto
    {
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public string? ImagenUrl { get; set; }
    }




}