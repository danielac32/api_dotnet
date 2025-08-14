// Controllers/resumenController.cs
using Microsoft.AspNetCore.Mvc;
using backend_ont_2.shared.apiResponse;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace backend_ont_2.features.media.controller.resumen
{
    [ApiController]
    [Route("media/resumen")]
    public class ResumenController : ControllerBase
    {
        private readonly ApiResponseService _apiResponseService;
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] ImageExtensions = { "png", "jpg", "jpeg", "gif", "webp", "svg" };

        public ResumenController(ApiResponseService apiResponseService, IWebHostEnvironment environment)
        {
            _apiResponseService = apiResponseService;
            _environment = environment;
        }

        private string GetResumenPath()
        {
            return Path.Combine(_environment.ContentRootPath, "assets", "resumen");
        }

        private bool IsImageFile(string filename)
        {
            var ext = Path.GetExtension(filename).TrimStart('.').ToLowerInvariant();
            return ImageExtensions.Contains(ext);
        }

        private string SanitizeFilename(string filename)
        {
            return System.Text.RegularExpressions.Regex.Replace(filename, @"[^a-zA-Z0-9_.-]", "_")
                                                       .Replace("__", "_");
        }

        // ===================================================
        // 🔹 POST: /api/resumen/upload
        // Sube una imagen en Base64 al resumen
        // ===================================================

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] FileUploadRequest request)
        {
            try
            {
                // Validaciones básicas
                if (request?.File == null)
                {
                    return BadRequest(new { success = false, message = "Datos de archivo no proporcionados" });
                }

                if (string.IsNullOrWhiteSpace(request.File.Filename))
                {
                    return BadRequest(new { success = false, message = "Nombre de archivo no proporcionado" });
                }

                if (string.IsNullOrWhiteSpace(request.File.Content))
                {
                    return BadRequest(new { success = false, message = "Contenido Base64 no proporcionado" });
                }

                // Decodificar Base64
                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(request.File.Content);
                }
                catch (FormatException)
                {
                    return BadRequest(new { success = false, message = "Formato Base64 inválido" });
                }

                // Crear directorio si no existe
                var uploadsDir = GetResumenPath();
                Directory.CreateDirectory(uploadsDir);

                // Generar nombre único
                var fileExt = Path.GetExtension(request.File.Filename);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExt}";
                var filePath = Path.Combine(uploadsDir, uniqueFileName);

                // Guardar archivo
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                // Retornar respuesta exitosa
                return Ok(new 
                {
                    success = true,
                    path = $"{filePath}/{uniqueFileName}",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error interno: {ex.Message}" });
            }
        }

        // ===================================================
        // 🔹 GET: /api/resumen/list
        // Lista todas las imágenes del resumen
        // ===================================================
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            return await _apiResponseService.Execute(async () =>
            {
                var resumenDir = GetResumenPath();

                if (!Directory.Exists(resumenDir))
                    return _apiResponseService.NotFoundResponse("Carpeta resumen no encontrada");

                var files = Directory.GetFiles(resumenDir)
                    .Where(IsImageFile)
                    .Select(f => Path.GetFileName(f))
                    .ToList();

                var response = new
                {
                    success = true,
                    count = files.Count,
                    list = files,
                    timestamp = DateTime.UtcNow.ToString("o")
                };
                return Ok(response);
            });
        }

        // ===================================================
        // 🔹 GET: /api/resumen/image?name=img_123.jpg
        // Sirve una imagen por nombre
        // ===================================================
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Nombre de imagen requerido" });

            name = SanitizeFilename(name);
            var filePath = Path.Combine(GetResumenPath(), name);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "Imagen no encontrada" });

            var ext = Path.GetExtension(name).TrimStart('.').ToLowerInvariant();
            var contentType = ext switch
            {
                "png" => "image/png",
                "jpg" or "jpeg" => "image/jpeg",
                "gif" => "image/gif",
                "webp" => "image/webp",
                "svg" => "image/svg+xml",
                _ => "image/png"
            };

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, contentType);
        }

        // ===================================================
        // 🔹 DELETE: /api/resumen/remove?name=img_123.jpg
        // Elimina una imagen del resumen
        // ===================================================
        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] string name)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(name))
                    return _apiResponseService.BadRequestResponse("Parámetro 'name' es requerido");

                name = SanitizeFilename(name);
                var filePath = Path.Combine(GetResumenPath(), name);

                if (!System.IO.File.Exists(filePath))
                    return _apiResponseService.NotFoundResponse("Imagen no encontrada");

                try
                {
                    System.IO.File.Delete(filePath);
                    var response = new
                    {
                        success = true,
                        message = "Imagen eliminada correctamente"
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return _apiResponseService.ConflictResponse($"Error al eliminar imagen: {ex.Message}");
                }
            });
        }
    }
}