 
using Microsoft.AspNetCore.Mvc;

using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using backend_ont_2.Xmltxt.DataClass;
using backend_ont_2.OracleDbProject;
 using backend_ont_2.Xmltxt.Service;
using System.Globalization;
 
 
 
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;


namespace backend_ont_2.Xmltxt.controller;

[ApiController]
[Route("/api/xmltxt/")]
public class XmltxtController : ControllerBase
{
    private readonly OracleDb _oracleDb;
    private readonly ApiResponseService _apiResponseService;

    private readonly IWebHostEnvironment _environment;

    private readonly XmlService _xmlService;
    public XmltxtController(XmlService xmlService, OracleDb oracleDb, ApiResponseService apiResponseService, IWebHostEnvironment environment)
    {

        _oracleDb = oracleDb;
        _apiResponseService = apiResponseService;
        _environment = environment;
        _xmlService = xmlService;
    }

    [HttpGet("process")]
    public async Task<IActionResult> ProcessDir()
    {
        return await _apiResponseService.Execute(async () =>
        {
            var fileEntries = await _xmlService.GetXmlFiles();
            if (fileEntries.Count > 0)
            {
                var result = await _xmlService.ProcessXmlFiles();

                return Ok(result);
            }
            else
            {
                return _apiResponseService.NotFoundResponse("No hay archivos XML para procesar.");
            }
        });
    }


    private string GetXmlUploadPath()
    {
        // Carpeta donde se guardarán los XML: /assets/xml
        return Path.Combine(_environment.ContentRootPath, "xml");
    }



    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromBody] FileUploadRequest request)
    {
        try
        {
            // Validaciones básicas
            if (request?.File == null)
                return BadRequest(new { success = false, message = "Datos de archivo no proporcionados" });

            if (string.IsNullOrWhiteSpace(request.File.Filename))
                return BadRequest(new { success = false, message = "Nombre de archivo no proporcionado" });

            if (string.IsNullOrWhiteSpace(request.File.Content))
                return BadRequest(new { success = false, message = "Contenido Base64 no proporcionado" });

            // ✅ Validar que sea .xml
            var extension = Path.GetExtension(request.File.Filename).TrimStart('.').ToLowerInvariant();
            if (extension != "xml")
            {
                return BadRequest(new { success = false, message = "Solo se permiten archivos XML (.xml)" });
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
            var uploadPath = GetXmlUploadPath();
            Directory.CreateDirectory(uploadPath);

            // Generar nombre único (opcional: puedes usar el original si prefieres)
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.File.Filename)}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            // Guardar archivo
            await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

            return Ok(new
            {
                success = true,
                message = "Archivo XML subido correctamente",
                fileName = uniqueFileName,
                originalName = request.File.Filename,
                fileSize = fileBytes.Length
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = $"Error al guardar el archivo: {ex.Message}" });
        }
    }
   


    [HttpGet("list")]
    public async Task<IActionResult> ListDir()
    {
        return await _apiResponseService.Execute(async () =>
        {
            var fileEntries = await _xmlService.GetXmlFiles();
            if (fileEntries.Count > 0)
            {
                var response = new
                {
                    res = new
                    {
                        size = fileEntries.Count,
                        files = fileEntries
                    }
                };

                return Ok(response);
            }
            else
            {
                var response = new
                {
                    res = new
                    {
                        size = 0,
                        files = new List<string>()
                    }
                };

                return Ok(response);
            }
        });


    }









}