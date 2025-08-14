using System.ComponentModel.DataAnnotations;
public class NameDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    //[MinLength(6, ErrorMessage = "El nombre debe tener al menos 6 caracteres.")]
     public string name { get; set; }
}