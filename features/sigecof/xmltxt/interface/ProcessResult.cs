public class ProcessResult
{
    public string Archivo { get; set; } = string.Empty;
    public int Planillas { get; set; }
    public int Errores { get; set; }
    public string? Detalle { get; set; } // Solo si hay error crítico
}