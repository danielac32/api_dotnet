 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.features.user.dto;
using backend_ont_2.model;
using backend_ont_2.features.user.service;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 


namespace backend_ont_2.features.user.controller.user
{
    [ApiController]
    [Route("user/")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ApiResponseService _apiResponseService;


        public UsersController(UserService userService, ApiResponseService apiResponseService)
        {
            _userService = userService;
            _apiResponseService = apiResponseService;

        }

        // GET: /user/get  → sin auth
        [HttpGet("get")]
        public async Task<IActionResult> List()
        {
            /*var users = new[]
            {
                new { id = 1, name = "Usuario Público", email = "publico@ejemplo.com" }
            };

            return Ok(new
            {
                success = true,
                users = users
            });*/

            return await _apiResponseService.Execute(async () =>
            {
                if (!ModelState.IsValid)
                    return _apiResponseService.BadRequestResponse("Datos de entrada inválidos");

                List<User> users = await _userService.getUserByFilter(null, null, null);

                return _apiResponseService.OkResponse(data: users);
            });


        }

        // GET: /user  → con auth
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListWithAuth()
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (!ModelState.IsValid)
                    return _apiResponseService.BadRequestResponse("Datos de entrada inválidos");

                List<User> users = await _userService.getUserByFilter(null, null, null);

                return _apiResponseService.OkResponse(data: users);
            });
        }

        // GET: /user/filter?status=active
        [HttpGet("filter")]
        //[Authorize]
        public async Task<IActionResult> ListFilter([FromQuery] string? status/*, [FromQuery] string? role*/)
        {
            // Simula que usas los query params
            /*return Ok(new
            {
                success = true,
                users = new[]
                {
                    new { filter = new { status, role }, data = "datos simulados" }
                }
            });*/
            return await _apiResponseService.Execute(async () =>
            {
                if (!ModelState.IsValid)
                    return _apiResponseService.BadRequestResponse("Datos de entrada inválidos");

                List<User> users = await _userService.getUserByFilter(null, null, status: status);

                return _apiResponseService.OkResponse(data: users);
            });

        }

        // GET: /user/{id}
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            return await _apiResponseService.Execute(async () =>
            {

                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                User user = await _userService.GetUserByIdOrEmailAsync(id);

                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");
                return _apiResponseService.OkResponse(data: user);
            });
        }

        // PATCH: /user/{id}
        [HttpPatch("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateById(
            [FromRoute] string id,
            [FromBody] UserUpdateDto updateDto) // Puedes usar dynamic o un DTO real
        {

            return await _apiResponseService.Execute(async () =>
           {
               if (string.IsNullOrWhiteSpace(id))
                   return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

               User user = await _userService.GetUserByIdOrEmailAsync(id);

               if (user == null)
                   return _apiResponseService.NotFoundResponse("Usuario no encontrado");

               user.Name = updateDto.Name ?? user.Name;
               user.Email = updateDto.Email ?? user.Email;
               user.Password = updateDto.Password ?? user.Password;
               user.Role = updateDto.Role ?? user.Role;
               user.Department = updateDto.Department ?? user.Department;
               user.IsActive = updateDto.IsActive ?? user.IsActive;
               user.Position = updateDto.Position ?? user.Position;
               user.ProfileImage = updateDto.ProfileImage ?? user.ProfileImage;
               user.UpdatedAt = DateTime.UtcNow;

               var updatedUser = await _userService.UpdateUserAsync(user);

               return _apiResponseService.OkResponse(data: updatedUser);
           });
        }

        // DELETE: /user/{id}
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteById([FromRoute] string id)
        {
            return await _apiResponseService.Execute(async () =>
           {
               if (string.IsNullOrWhiteSpace(id))
                   return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

               User user = await _userService.GetUserByIdOrEmailAsync(id);

               if (user == null)
                   return _apiResponseService.NotFoundResponse("Usuario no encontrado");


               var delete = _userService.DeleteUserAsync(user.Id);
               return _apiResponseService.OkResponse(message: "Usuario eliminado");
           });
        }
        /***********************************************************************************************************/
// ===================================================
        // 🔹 GET: /user/{id}/permissions
        // Obtiene todos los permisos de un usuario
        // ===================================================
        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetPermissionsByUser([FromRoute] string id)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                User user = await _userService.GetUserByIdOrEmailAsync(id);

                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");
                   
                return _apiResponseService.OkResponse();

               // var permissions = await _userService.GetPermissionsByUserAsync(id);
                ///return _apiResponseService.OkResponse(permissions, "Permisos obtenidos correctamente");
            });
        }

        // ===================================================
        // 🔹 POST: /user/{id}/permissions
        // Agrega un nuevo permiso al usuario
        // ===================================================
        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AddPermissionToUser(
            [FromRoute] string id,
            [FromBody] PermissionDto dto)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                User user = await _userService.GetUserByIdOrEmailAsync(id);

                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");
                   
                return _apiResponseService.OkResponse();

                /*var permission = new Permission
                {
                    Section = dto.Section,
                    CanCreate = dto.CanCreate,
                    CanEdit = dto.CanEdit,
                    CanDelete = dto.CanDelete,
                    CanPublish = dto.CanPublish,
                    UserId = id
                };

                bool added = await _userService.AddPermissionToUserAsync(id, permission);
                if (!added)
                    return _apiResponseService.BadRequestResponse("No se pudo agregar el permiso");

                return _apiResponseService.OkResponse(permission, "Permiso agregado correctamente");*/
            });
        }

        // ===================================================
        // 🔹 DELETE: /user/{id}/permissions/{permissionId}
        // Elimina un permiso específico del usuario
        // ===================================================
        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermissionFromUser(
            [FromRoute] string id,
            [FromRoute] int permissionId)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                User user = await _userService.GetUserByIdOrEmailAsync(id);

                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");
                   

                return _apiResponseService.OkResponse();

                /*bool removed = await _userService.RemovePermissionFromUserAsync(id, permissionId);
                if (!removed)
                    return _apiResponseService.NotFoundResponse("Permiso no encontrado o no pertenece al usuario");

                return _apiResponseService.OkResponse(null, "Permiso eliminado correctamente");*/
            });
        }

        // ===================================================
        // 🔹 PATCH: /user/{id}/permissions
        // Actualiza un permiso existente
        // ===================================================
        [HttpPatch("{id}/permissions")]
        public async Task<IActionResult> UpdatePermissionFromUser(
            [FromRoute] string id,
            [FromBody] UpdatePermissionDto dto)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                User user = await _userService.GetUserByIdOrEmailAsync(id);

                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");


                return _apiResponseService.OkResponse();
                /*var permission = await _userService.GetPermissionByIdAsync(dto.PermissionId);
                if (permission == null || permission.UserId != id)
                    return _apiResponseService.NotFoundResponse("Permiso no encontrado o no pertenece al usuario");

                // Actualizar solo los campos enviados
                if (dto.CanCreate.HasValue) permission.CanCreate = dto.CanCreate.Value;
                if (dto.CanEdit.HasValue) permission.CanEdit = dto.CanEdit.Value;
                if (dto.CanDelete.HasValue) permission.CanDelete = dto.CanDelete.Value;
                if (dto.CanPublish.HasValue) permission.CanPublish = dto.CanPublish.Value;

                bool updated = await _userService.UpdatePermissionAsync(permission);
                if (!updated)
                    return _apiResponseService.InternalServerError("Error al actualizar el permiso");

                return _apiResponseService.OkResponse(permission, "Permiso actualizado correctamente");*/
            });
        }
        /***********************************************************************************************************/

        /***********************************************************************************************************/

        /***********************************************************************************************************/
    }

}