using Microsoft.AspNetCore.Mvc;
using backend_ont_2.data;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using backend_ont_2.OracleDbProject;
using backend_ont_2.sigecof.sql.parser;


namespace backend_ont_2.sigecof.cuenta_unica.controller;

[ApiController]
[Route("/api/query")]

public class CuentaUnicaController : ControllerBase
{
    private readonly OracleDb _oracleDb;
    private readonly ApiResponseService _apiResponseService;

    public CuentaUnicaController(OracleDb oracleDb, ApiResponseService apiResponseService)
    {
        _oracleDb = oracleDb;
        _apiResponseService = apiResponseService;
    }

    [HttpPost("comprobante_de_retencion")]
    public async Task<IActionResult> comprobante_de_retencion([FromQuery] FechaRangoDto fecha)
    {
        return await _apiResponseService.Execute(async () =>
        {

            string sql = SqlFileLoader.LoadFile("sql/CuentaUnica", "COMPROBANTE_DE_RETENCION.sql", fecha.desde, fecha.hasta);
            var result = await _oracleDb.QueryReadOnly(sql);
            return Ok(result);
        });
    }

    [HttpPost("parafiscales_banavih")]
    public async Task<IActionResult> parafiscales_banavih([FromQuery] FechaRangoDto fecha)
    {
        return await _apiResponseService.Execute(async () =>
        {

            string sql = SqlFileLoader.LoadFile("sql/CuentaUnica", "PARAFISCALES_BANAVIH.sql", fecha.desde, fecha.hasta);
            var result = await _oracleDb.QueryReadOnly(sql);
            return Ok(result);
        });
    }

    [HttpPost("parafiscales_inces")]
    public async Task<IActionResult> parafiscales_inces([FromQuery] FechaRangoDto fecha)
    {
        return await _apiResponseService.Execute(async () =>
        {

            string sql = SqlFileLoader.LoadFile("sql/CuentaUnica", "PARAFISCALES_INCES.sql", fecha.desde, fecha.hasta);
            var result = await _oracleDb.QueryReadOnly(sql);
            return Ok(result);
        });
    }
    [HttpPost("parafiscales_ivss")]
    public async Task<IActionResult> parafiscales_ivss([FromQuery] FechaRangoDto fecha)
    {
        return await _apiResponseService.Execute(async () =>
        {

            string sql = SqlFileLoader.LoadFile("sql/CuentaUnica", "PARAFISCALES_IVSS.sql", fecha.desde, fecha.hasta);
            var result = await _oracleDb.QueryReadOnly(sql);
            return Ok(result);
        });
    }
    [HttpPost("retenciones")]

    public async Task<IActionResult> retenciones([FromQuery] FechaRangoDto fecha)
    {
        return await _apiResponseService.Execute(async () =>
        {

            string sql = SqlFileLoader.LoadFile("sql/CuentaUnica", "RETENCIONES.sql", fecha.desde, fecha.hasta);
            var result = await _oracleDb.QueryReadOnly(sql);
            return Ok(result);
        });
    }

    [HttpPost("islr")]

    public async Task<IActionResult> islr([FromQuery] FechaRangoDto fecha)
    {
        return await _apiResponseService.Execute(async () =>
        {

            string sql = SqlFileLoader.LoadFile("sql/CuentaUnica","ISLR.sql", fecha.desde, fecha.hasta);
            var result = await _oracleDb.QueryReadOnly(sql);
            return Ok(result);
        });
    }

}