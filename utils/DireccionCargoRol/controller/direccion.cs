using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.model;
using backend_ont_2.DCR.Services;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


namespace backend_ont_2.features.dcr.controller.Dir
{
    [ApiController]
    [Route("direccion/")]
    public class DireccionController : ControllerBase
    {

        private readonly DireccionCargoRolService _direccion;
        private readonly ApiResponseService _apiResponseService;

        public DireccionController(DireccionCargoRolService direccionCargoRolService, ApiResponseService apiResponseService)
        {
            _direccion = direccionCargoRolService;
            _apiResponseService = apiResponseService;
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] NameDto dto) {
            return await _apiResponseService.Execute(async () =>
            {
                if (await _direccion.DireccionExists(dto.name))
                    return _apiResponseService.ConflictResponse("La dirección ya existe");

                var direccion = new Direccion(dto.name);
                bool created = await _direccion.CreateDireccion(direccion);

                return created
                    ? _apiResponseService.OkResponse(null, "Dirección registrada exitosamente")
                    : _apiResponseService.BadRequestResponse("Error al registrar");
            });
        }
        [HttpGet]
        public async Task<IActionResult> list() {
            return await _apiResponseService.Execute(async () =>
            {
                List<Direccion> dirs = await _direccion.GetAllDirecciones();

                return _apiResponseService.OkResponse(data: dirs);
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getById([FromRoute] int id) {
            return await _apiResponseService.Execute(async () =>
            {
                if (id < 0)
                {
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");
                }
                Direccion dir = await _direccion.GetDireccionById(id);

                if (dir == null)
                    return _apiResponseService.NotFoundResponse("Direccion no encontrada");
                return _apiResponseService.OkResponse(data: dir);
            });
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> update([FromBody] NameDto dto,[FromRoute] int id) {
            return await _apiResponseService.Execute(async () =>
            {
                if (id < 0)
                {
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");
                }
                Direccion dir = await _direccion.GetDireccionById(id);

                if (dir == null)
                    return _apiResponseService.NotFoundResponse("Direccion no encontrada");

                dir.Name = dto.name;
                var updatedDir = await _direccion.UpdateDireccion(dir);

                return _apiResponseService.OkResponse(data: updatedDir);
            });     
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete([FromRoute] int id) {
            return await _apiResponseService.Execute(async () =>
            {
                if (id < 0)
                {
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");
                }
                Direccion dir = await _direccion.GetDireccionById(id);

                if (dir == null)
                    return _apiResponseService.NotFoundResponse("Direccion no encontrada");

                
                var delete = await _direccion.DeleteDireccion(dir.Id);

                return _apiResponseService.OkResponse(message: "Direccion eliminada");
            });     
        }

    }
}