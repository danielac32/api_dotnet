 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
 
using System.Net.Mime;
using backend_ont_2.OracleDbProject;
using backend_ont_2.sigecof.sql.parser;
 
namespace backend_ont_2.sigecof.planificacion.controller
{
    [ApiController]
    [Route("/api/query/")]
    public class PlanificacionController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly OracleDb _oracleDb;
        private readonly ApiResponseService _apiResponseService;
        public PlanificacionController(IWebHostEnvironment environment, OracleDb oracleDb, ApiResponseService apiResponseService)
        {
            _environment = environment;
            _oracleDb = oracleDb;
            _apiResponseService = apiResponseService;
        }

        // GET: /user/get  → sin auth
        [HttpPost("transmisiones")]
        //[Authorize]
        public async Task<IActionResult> transmisiones([FromQuery] FechaRangoDto fecha)
        {

            return await _apiResponseService.Execute(async () =>
            {
                /* _oracleDb.Connect("10.79.6.247:1521/SIGEPROD.oncop.gob.ve", "Consulta", "pumyra1584");
                 string sql = SqlFileLoader.LoadFile("TRANSMISIONES_ORDENES.sql", fecha.desde, fecha.hasta);
                 var result = await _oracleDb.ExecuteQuery(sql);
                 _oracleDb.Close();
                 return Ok(result);*/
                string sql = SqlFileLoader.LoadFile("TRANSMISIONES_ORDENES.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("pagadas2")]
        //[Authorize]
        public async Task<IActionResult> pagadas2([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {
                /* _oracleDb.Connect("10.79.6.247:1521/SIGEPROD.oncop.gob.ve", "Consulta", "pumyra1584");
                string sql = SqlFileLoader.LoadFile("ORDENES_PAGADAS.sql", fecha.desde, fecha.hasta);
               var result =await  _oracleDb.ExecuteQuery(sql);
                _oracleDb.Close();
                return Ok(result);*/
                string sql = SqlFileLoader.LoadFile("PAGADAS_PLANIFICACION2.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("pagadas_partidas")]
        //[Authorize]
        public async Task<IActionResult> pagadas_partidas([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {
                /* _oracleDb.Connect("10.79.6.247:1521/SIGEPROD.oncop.gob.ve", "Consulta", "pumyra1584");
                string sql = SqlFileLoader.LoadFile("ORDENES_PAGADAS.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.ExecuteQuery(sql);
                _oracleDb.Close();
                return Ok(result);*/
                string sql = SqlFileLoader.LoadFile("PAGADAS_PLANIFICACION2.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("pagadas_resumen")]
        //[Authorize]
        public async Task<IActionResult> pagadas_resumen([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {
                /* _oracleDb.Connect("10.79.6.247:1521/SIGEPROD.oncop.gob.ve", "Consulta", "pumyra1584");
                string sql = SqlFileLoader.LoadFile("ORDENES_PAGADAS.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.ExecuteQuery(sql);
                _oracleDb.Close();
                return Ok(result);*/
                string sql = SqlFileLoader.LoadFile("PAGADAS_RESUMEN.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("ordenes-pendientes")]
        //[Authorize]
        public async Task<IActionResult> ordenes_pendientes([FromQuery] FechaRangoDto fecha)
        {
            //Console.WriteLine($"desde: {desde}, hasta: {hasta}"); // Verifica valores
            // Validación manual
            //Console.WriteLine($"ordenes pendientes: {fecha.desde} {fecha.hasta}");
            return await _apiResponseService.Execute(async () =>
            {
                /* _oracleDb.Connect("10.79.6.247:1521/SIGEPROD.oncop.gob.ve", "Consulta", "pumyra1584");
                 string sql = SqlFileLoader.LoadFile("ORDENES_PENDIENTES_CON_BENEFICIARIO.sql", fecha.desde, fecha.hasta);
                 //Console.WriteLine(sql);
                 var result = await _oracleDb.ExecuteQuery(sql);
                 _oracleDb.Close();
                 return Ok(result);*/
                string sql = SqlFileLoader.LoadFile("ORDENES_PENDIENTES_CON_BENEFICIARIO.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }

        [HttpPost("ordenes-divisas-bolivares")]
        //[Authorize]
        public async Task<IActionResult> ordenes_divisas_bolivares([FromQuery] FechaRangoDto fecha)
        {
            return await _apiResponseService.Execute(async () =>
            {
                /* _oracleDb.Connect("10.79.6.247:1521/SIGEPROD.oncop.gob.ve", "Consulta", "pumyra1584");
                string sql = SqlFileLoader.LoadFile("Dolares_a_Bolivares.sql",fecha.desde, fecha.hasta);
                var result = await _oracleDb.ExecuteQuery(sql);
                _oracleDb.Close();
                return Ok(result);*/
                string sql = SqlFileLoader.LoadFile("Dolares_a_Bolivares.sql", fecha.desde, fecha.hasta);
                var result = await _oracleDb.QueryReadOnly(sql);
                return Ok(result);
            });
        }
    }
}