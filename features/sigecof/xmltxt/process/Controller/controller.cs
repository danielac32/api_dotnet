 
using Microsoft.AspNetCore.Mvc;

using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using backend_ont_2.Xmltxt.DataClass;
using backend_ont_2.OracleDbProject;
 using backend_ont_2.Xmltxt.Service;
 

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
        
            if (fileEntries.Count == 0)
                return _apiResponseService.NotFoundResponse("No hay archivos XML para procesar.");

            var result = await _xmlService.ProcessXmlFiles();
            return Ok(result);
        });
    }



    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromBody] FileUploadRequestList request)
    {
        return await _apiResponseService.Execute(async () =>
        {
            var result = await _xmlService.UploadXmlFilesList(request);
            return Ok(result);
        });
    }



    [HttpGet("list")]
    public async Task<IActionResult> ListDir()
    {
        return await _apiResponseService.Execute(async () =>
        {
            var fileEntries = await _xmlService.GetXmlFiles();
            
            var response = new
            {
                res = new
                {
                    size = fileEntries.Count,
                    files = fileEntries
                }
            };

            return Ok(response);
        });
    }

    
    [HttpDelete("deleteFile")]
    public async Task<IActionResult> Remove([FromQuery] string name)
    {
        return await _apiResponseService.Execute(async () =>
        {

            var deleted = await _xmlService.Remove(name);
            return Ok(deleted);
            /*if (string.IsNullOrWhiteSpace(name))
                return _apiResponseService.BadRequestResponse("Parámetro 'name' es requerido");

            
            var filePath = Path.Combine(_xmlService.GetXmlUploadPath(), name);

            if (!System.IO.File.Exists(filePath))
                return _apiResponseService.NotFoundResponse("archivo no encontrado");

             
            System.IO.File.Delete(filePath);
            var response = new
            {
                success = true,
                message = "xml eliminado correctamente"
            };
            return Ok(response);*/
        });
    }








}