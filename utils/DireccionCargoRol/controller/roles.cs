using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.model;
using backend_ont_2.DCR.Services;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


namespace backend_ont_2.features.dcr.controller.Roles
{
    [ApiController]
    [Route("role/")]
    public class RolesController : ControllerBase
    {

        private readonly DireccionCargoRolService _roles;
        private readonly ApiResponseService _apiResponseService;

        public RolesController(DireccionCargoRolService direccionCargoRolService, ApiResponseService apiResponseService)
        {
            _roles = direccionCargoRolService;
            _apiResponseService = apiResponseService;
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] NameDto dto) {
            return await _apiResponseService.Execute(async () =>
            {
                if (await _roles.RoleExists(dto.name))
                    return _apiResponseService.ConflictResponse("El rol ya existe");

                var rol = new Role(dto.name);
                bool created = await _roles.CreateRole(rol);

                return created
                    ? Ok(new { success=true,message="Rol registrado exitosamente"})//_apiResponseService.OkResponse(null, "Rol registrado exitosamente")
                    : _apiResponseService.BadRequestResponse("Error al registrar");
            });
        }

        [HttpGet]
        public async Task<IActionResult> list() {
            return await _apiResponseService.Execute(async () =>
            {
                List<Role> roles = await _roles.GetAllRoles();

                //return _apiResponseService.OkResponse(data: roles);
                var response = new
                {
                    succes = true,
                    roles
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
                Role rol = await _roles.GetRoleById(id);

                if (rol == null)
                    return _apiResponseService.NotFoundResponse("Rol no encontrada");
                return _apiResponseService.OkResponse(data: rol);
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
                Role rol = await _roles.GetRoleById(id);

                if (rol == null)
                    return _apiResponseService.NotFoundResponse("Rol no encontrada");

                rol.Name = dto.name;
                var updatedCargo = await _roles.UpdateRole(rol);

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
                Role rol = await _roles.GetRoleById(id);

                if (rol == null)
                    return _apiResponseService.NotFoundResponse("Rol no encontrada");

                
                var delete = await _roles.DeleteDireccion(rol.Id);

                return _apiResponseService.OkResponse(message: "Rol eliminada");
            });     
        }

    }
}