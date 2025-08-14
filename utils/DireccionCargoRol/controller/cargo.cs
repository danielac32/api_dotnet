using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.model;
using backend_ont_2.DCR.Services;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


namespace backend_ont_2.features.dcr.controller.Cargos
{
    [ApiController]
    [Route("cargo/")]
    public class CargoController : ControllerBase
    {

        private readonly DireccionCargoRolService _cargo;
        private readonly ApiResponseService _apiResponseService;

        public CargoController(DireccionCargoRolService direccionCargoRolService, ApiResponseService apiResponseService)
        {
            _cargo = direccionCargoRolService;
            _apiResponseService = apiResponseService;
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] NameDto dto) {
            return await _apiResponseService.Execute(async () =>
            {
                if (await _cargo.CargoExists(dto.name))
                    return _apiResponseService.ConflictResponse("El cargo ya existe");

                var cargo = new Cargo(dto.name);
                bool created = await _cargo.CreateCargo(cargo);

                return created
                    ? _apiResponseService.OkResponse(null, "Cargo registrado exitosamente")
                    : _apiResponseService.BadRequestResponse("Error al registrar");
            });
        }

        [HttpGet]
        public async Task<IActionResult> list() {
            return await _apiResponseService.Execute(async () =>
            {
                List<Cargo> cargos = await _cargo.GetAllCargos();

                //return _apiResponseService.OkResponse(data: cargos);
                var response = new
                {
                    succes = true,
                    cargos
                };
                return Ok(response);
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
                Cargo cargo = await _cargo.GetCargoById(id);

                if (cargo == null)
                    return _apiResponseService.NotFoundResponse("Cargo no encontrada");
                return _apiResponseService.OkResponse(data: cargo);
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
                Cargo cargo = await _cargo.GetCargoById(id);

                if (cargo == null)
                    return _apiResponseService.NotFoundResponse("Cargo no encontrada");

                cargo.Name = dto.name;
                var updatedCargo = await _cargo.UpdateCargo(cargo);

                return _apiResponseService.OkResponse(data: updatedCargo);
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
                Cargo cargo = await _cargo.GetCargoById(id);

                if (cargo == null)
                    return _apiResponseService.NotFoundResponse("Cargo no encontrada");

                
                var delete = await _cargo.DeleteDireccion(cargo.Id);

                return _apiResponseService.OkResponse(message: "Cargo eliminada");
            });     
        }

    }
}