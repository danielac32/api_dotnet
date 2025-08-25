using System.ComponentModel.DataAnnotations;
public class FechaRangoDto
{
    [Required(ErrorMessage = "El parámetro 'desde' es obligatorio.")]
    public string? desde { get; set; }

    [Required(ErrorMessage = "El parámetro 'hasta' es obligatorio.")]
    public string? hasta { get; set; }
}