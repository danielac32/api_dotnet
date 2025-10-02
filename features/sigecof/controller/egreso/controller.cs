 
using Microsoft.AspNetCore.Mvc;

using backend_ont_2.data;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using backend_ont_2.OracleDbProject;
using backend_ont_2.sigecof.sql.parser;
 

namespace backend_ont_2.sigecof.egreso.controller
{
    [ApiController]
    [Route("/api/query/")]
    public class EgresoController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly OracleDb _oracleDb;
        private readonly ApiResponseService _apiResponseService;
        public EgresoController(IWebHostEnvironment environment, OracleDb oracleDb, ApiResponseService apiResponseService)
        {
            _environment = environment;
            _oracleDb = oracleDb;
            _apiResponseService = apiResponseService;
        }

        // GET: /user/get  → sin auth
        [HttpPost("pendientes")]
        //[Authorize]
        public async Task<IActionResult> pendientes([FromQuery] FechaRangoDto fecha)
        {

            return await _apiResponseService.Execute(async () =>
            {

                string sql = SqlFileLoader.LoadFile("pendientes.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("pagadas")]
        //[Authorize]
        public async Task<IActionResult> pagadas([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {
                string sql = SqlFileLoader.LoadFile("pagadas.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);

            });
        }

        [HttpPost("pagadas-retenciones")]
        //[Authorize]
        public async Task<IActionResult> pagadas_retenciones([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {

                string sql = SqlFileLoader.LoadFile("pagadas_retenciones.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("retenciones-partidas")]
        //[Authorize]
        public async Task<IActionResult> retenciones_partidas([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {

                string sql = SqlFileLoader.LoadFile("retenciones_partidas.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("detalles-pendientes")]
        //[Authorize]
        public async Task<IActionResult> detalles_pendientes([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {
                string sql = SqlFileLoader.LoadFile("DETALLE_ORDENES_PENDIENTES.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);

            });
        }

        [HttpGet("consult-pendiente")]
        //[Authorize]
        public async Task<IActionResult> consult_pendientes([FromQuery] BusquedaPendiente request)
        {
            return await _apiResponseService.Execute(async () =>
            {
                // Validar que al menos un parámetro esté presente
                if (string.IsNullOrEmpty(request.orden) && string.IsNullOrEmpty(request.rif))
                {
                    return BadRequest("Debe proporcionar al menos un número de orden o RIF para la búsqueda");
                }

                string sql = SqlFileLoader.LoadFileOrdenRif("consult_pendiente.sql", request.orden, request.rif);
                Console.WriteLine(sql);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);

            });
        }

         [HttpGet("consult")]
        //[Authorize]
        public async Task<IActionResult> consult()
        {
            return await _apiResponseService.Execute(async () =>
            {
                 
                string sql = SqlFileLoader.LoadFile("consult.sql");
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);

            });
        }
    }
}