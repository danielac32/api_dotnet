// Controllers/noticiasController.cs
using Microsoft.AspNetCore.Mvc;
using backend_ont_2.shared.apiResponse;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace backend_ont_2.features.media.controller.noticias
{
    [ApiController]
    [Route("noticias")]
    public class NoticiasController : ControllerBase
    {
        private readonly ApiResponseService _apiResponseService;
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] ImageExtensions = { "png", "jpg", "jpeg", "gif", "webp", "svg" };

        public NoticiasController(ApiResponseService apiResponseService, IWebHostEnvironment environment)
        {
            _apiResponseService = apiResponseService;
            _environment = environment;
        }

        private string GetNoticiasPath()
        {
            return Path.Combine(_environment.ContentRootPath, "assets", "noticias");
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
        // 🔹 POST: /api/noticias/upload
        // Sube una imagen en Base64 al noticias
        // ===================================================
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] UploadFileDto dto)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(dto?.Filename) || string.IsNullOrWhiteSpace(dto.Content))
                    return _apiResponseService.BadRequestResponse("Datos de archivo inválidos");

                var noticiasDir = GetNoticiasPath();
                Directory.CreateDirectory(noticiasDir); // Asegurar que exista

                byte[] bytes;
                try
                {
                    bytes = Convert.FromBase64String(dto.Content);
                }
                catch (FormatException)
                {
                    return _apiResponseService.BadRequestResponse("Contenido Base64 inválido");
                }

                var ext = Path.GetExtension(dto.Filename).ToLowerInvariant();
                var filename = $"img_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
                var filePath = Path.Combine(noticiasDir, filename);

                await System.IO.File.WriteAllBytesAsync(filePath, bytes);

                var imageUrl = $"/api/noticias/image?name={filename}";
                return _apiResponseService.OkResponse(new
                {
                    status = "success",
                    path = imageUrl
                }, "Imagen subida correctamente");
            });
        }

        // ===================================================
        // 🔹 GET: /api/noticias/list
        // Lista todas las imágenes del noticias
        // ===================================================
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            return await _apiResponseService.Execute(async () =>
            {
                var noticiasDir = GetNoticiasPath();

                if (!Directory.Exists(noticiasDir))
                    return _apiResponseService.NotFoundResponse("Carpeta noticias no encontrada");

                var files = Directory.GetFiles(noticiasDir)
                    .Where(IsImageFile)
                    .Select(f => Path.GetFileName(f))
                    .ToList();

                return _apiResponseService.OkResponse(new
                {
                    success = true,
                    count = files.Count,
                    list = files,
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            });
        }

        // ===================================================
        // 🔹 GET: /api/noticias/image?name=img_123.jpg
        // Sirve una imagen por nombre
        // ===================================================
        [HttpGet("image")]
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Nombre de imagen requerido" });

            name = SanitizeFilename(name);
            var filePath = Path.Combine(GetNoticiasPath(), name);

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
        // 🔹 DELETE: /api/noticias/remove?name=img_123.jpg
        // Elimina una imagen del noticias
        // ===================================================
        [HttpDelete("remove")]
        public async Task<IActionResult> Remove([FromQuery] string name)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(name))
                    return _apiResponseService.BadRequestResponse("Parámetro 'name' es requerido");

                name = SanitizeFilename(name);
                var filePath = Path.Combine(GetNoticiasPath(), name);

                if (!System.IO.File.Exists(filePath))
                    return _apiResponseService.NotFoundResponse("Imagen no encontrada");

                try
                {
                    System.IO.File.Delete(filePath);
                    return _apiResponseService.OkResponse(null, "Imagen eliminada correctamente");
                }
                catch (Exception ex)
                {
                    return _apiResponseService.ConflictResponse($"Error al eliminar imagen: {ex.Message}");
                }
            });
        }
    }
}