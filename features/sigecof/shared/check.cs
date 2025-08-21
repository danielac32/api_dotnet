

 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using backend_ont_2.OracleDbProject;
using System.Net.Mime;

namespace backend_ont_2.test_connection.controller
{

    [ApiController]
    [Route("/api/query/")]
    public class OracleCkeck:ControllerBase
    {
        private readonly OracleDb _oracleDb;

        public OracleCkeck(OracleDb oracleDb)
        {
            _oracleDb = oracleDb;
        }


        [HttpGet("connection")]
        //[Authorize]
        public async Task<IActionResult> checkConnection()
        {
            bool connectionStatus = false;
            try
            {
                _oracleDb.ConnectAsReadOnly();//Connect("10.79.6.247:1521/SIGEPROD.oncop.gob.ve", "Consulta", "pumyra1584");
                /*var result = await _oracleDb.ExecuteQuery("SELECT 'Prueba' AS msg, SYSDATE AS fecha FROM DUAL");
                foreach (var row in result)
                {
                    foreach (var kv in row)
                    {
                        Console.WriteLine($"{kv.Key}: {kv.Value}");
                    }
                }*/
                connectionStatus = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                connectionStatus = false;
            }
            finally
            {
                _oracleDb.Close();
            }
            return Ok(new {status=connectionStatus});
        }


    }
}