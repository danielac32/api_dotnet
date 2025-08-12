 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.features.user.dto;
using backend_ont_2.model;
using backend_ont_2.features.user.service;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace backend_ont_2.features.user.controller.user
{
    [ApiController]
    [Route("user/")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ApiResponseService _apiResponseService;
        private readonly IWebHostEnvironment _environment;

        public UsersController(UserService userService, ApiResponseService apiResponseService, IWebHostEnvironment environment)
        {
            _userService = userService;
            _apiResponseService = apiResponseService;
            _environment = environment;
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

                var res = new
                {
                    success = true,
                    users
                };
                return Ok(res);
                //return _apiResponseService.OkResponse(data: users);
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

                var permissions = await _userService.GetPermissionsByUserAsync(user.Id);
                return _apiResponseService.OkResponse(data: permissions);

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


                var permission = new Permission
                {
                    Section = dto.Section,
                    CanCreate = dto.CanCreate,
                    CanEdit = dto.CanEdit,
                    CanDelete = dto.CanDelete,
                    CanPublish = dto.CanPublish,
                    UserId = user.Id
                };

                bool added = await _userService.AddPermissionToUserAsync(user.Id, permission);
                if (!added)
                    return _apiResponseService.BadRequestResponse("No se pudo agregar el permiso");

                return _apiResponseService.OkResponse(permission, "Permiso agregado correctamente");
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

                bool removed = await _userService.RemovePermissionFromUserAsync(user.Id, permissionId);
                if (!removed)
                    return _apiResponseService.NotFoundResponse("Permiso no encontrado o no pertenece al usuario");

                return _apiResponseService.OkResponse(null, "Permiso eliminado correctamente");
            });
        }

        // ===================================================
        // 🔹 PATCH: /user/{id}/permissions
        // Actualiza un permiso existente
        // ===================================================
        [HttpPatch("{id}/permissions")]
        public async Task<IActionResult> UpdatePermissionFromUser(
            [FromRoute] int id,
            [FromBody] UpdatePermissionDto dto)
        {
            return await _apiResponseService.Execute(async () =>
             {
                 // Validar que el ID sea válido
                 if (id <= 0)
                     return _apiResponseService.BadRequestResponse("ID inválido");

                 // Buscar el permiso
                 var permission = await _userService.GetPermissionById(id);
                 if (permission == null)
                     return _apiResponseService.NotFoundResponse("Permiso no encontrado");

                 // Actualizar solo los campos enviados
                 if (dto.Section != null) permission.Section = dto.Section;
                 if (dto.CanCreate.HasValue) permission.CanCreate = dto.CanCreate.Value;
                 if (dto.CanEdit.HasValue) permission.CanEdit = dto.CanEdit.Value;
                 if (dto.CanDelete.HasValue) permission.CanDelete = dto.CanDelete.Value;
                 if (dto.CanPublish.HasValue) permission.CanPublish = dto.CanPublish.Value;

                 // Guardar cambios
                 bool updated = await _userService.UpdatePermissionAsync(permission);
                 if (!updated)
                     return _apiResponseService.ConflictResponse("No se pudo actualizar el permiso");

                 // ✅ Devolver el permiso actualizado (no el DTO)
                 return _apiResponseService.OkResponse(permission, "Permiso actualizado correctamente");
             });
        }
        /***********************************************************************************************************/
        // ===================================================
        // 🔹 GET: /user/{id}/organismos
        // Obtiene todos los organismos creados por un usuario
        // ===================================================
        [HttpGet("{id}/organismos")]
        public async Task<IActionResult> GetOrganismosByUser([FromRoute] string id)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                var organismos = await _userService.GetOrganismosByUserAsync(user.Id);
                return _apiResponseService.OkResponse(organismos, "Organismos obtenidos correctamente");
            });
        }
        /***********************************************************************************************************/
        // ===================================================
        // 🔹 POST: /user/{id}/organismos
        // Crea un nuevo organismo asociado al usuario
        // ===================================================
        [HttpPost("{id}/organismos")]
        public async Task<IActionResult> AddOrganismoToUser(
            [FromRoute] string id,
            [FromBody] OrganismoGobernacionCreateDto dto)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                // Mapear DTO a entidad
                var organismo = new OrganismoGobernacion(dto.Nombre, dto.Valor1, dto.Valor2, dto.Valor3)
                {
                    AutorId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                bool added = await _userService.AddOrganismoToUserAsync(user.Id, organismo);
                if (!added)
                    return _apiResponseService.BadRequestResponse("No se pudo agregar el organismo");

                return _apiResponseService.OkResponse(organismo, "Organismo creado correctamente");
            });
        }
        /***********************************************************************************************************/
        // ===================================================
        // 🔹 DELETE: /user/{id}/organismos/{organismoId}
        // Elimina un organismo específico (solo si pertenece al usuario)
        // ===================================================
        [HttpDelete("{id}/organismos/{organismoId}")]
        public async Task<IActionResult> RemoveOrganismoFromUser(
            [FromRoute] string id,
            [FromRoute] int organismoId)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                bool removed = await _userService.RemoveOrganismoFromUserAsync(user.Id, organismoId);
                if (!removed)
                    return _apiResponseService.NotFoundResponse("Organismo no encontrado o no pertenece al usuario");

                return _apiResponseService.OkResponse(null, "Organismo eliminado correctamente");
            });
        }
        /***********************************************************************************************************/
        // ===================================================
        // 🔹 GET: /user/{id}/resumenes
        // Obtiene todos los resúmenes de gestión del usuario
        // ===================================================
        [HttpGet("{id}/resumenes")]
        public async Task<IActionResult> GetResumenesByUser([FromRoute] string id)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                var resumenes = await _userService.GetResumenesByUserAsync(user.Id);
                return _apiResponseService.OkResponse(resumenes, "Resúmenes obtenidos correctamente");
            });
        }

        // ===================================================
        // 🔹 POST: /user/{id}/resumenes
        // Crea un nuevo resumen de gestión para el usuario
        // ===================================================
        [HttpPost("{id}/resumenes")]
        public async Task<IActionResult> AddResumenToUser(
            [FromRoute] string id,
            [FromBody] ResumenGestionCreateDto dto)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                var resumen = new ResumenGestion(dto.Titulo, dto.Descripcion, dto.ImagenUrl)
                {
                    AutorId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                bool added = await _userService.AddResumenToUserAsync(user.Id, resumen);
                if (!added)
                    return _apiResponseService.BadRequestResponse("No se pudo crear el resumen");

                return _apiResponseService.OkResponse(resumen, "Resumen creado correctamente");
            });
        }

        // ===================================================
        // 🔹 DELETE: /user/{id}/resumenes/{resumenId}
        // Elimina un resumen de gestión (solo si pertenece al usuario)
        // ===================================================
        [HttpDelete("{id}/resumenes/{resumenId}")]
        public async Task<IActionResult> RemoveResumenFromUser(
            [FromRoute] string id,
            [FromRoute] int resumenId)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                bool removed = await _userService.RemoveResumenFromUserAsync(user.Id, resumenId);
                if (!removed)
                    return _apiResponseService.NotFoundResponse("Resumen no encontrado o no pertenece al usuario");

                return _apiResponseService.OkResponse(null, "Resumen eliminado correctamente");
            });
        }
        /***********************************************************************************************************/
        // ===================================================
        // 🔹 GET: /user/{id}/noticias
        // Obtiene todas las noticias del usuario
        // ===================================================
        [HttpGet("{id}/noticias")]
        public async Task<IActionResult> GetNoticiasByUser([FromRoute] string id)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                var noticias = await _userService.GetNoticiasByUserAsync(user.Id);
                return _apiResponseService.OkResponse(noticias, "Noticias obtenidas correctamente");
            });
        }

        // ===================================================
        // 🔹 POST: /user/{id}/noticias
        // Crea una nueva noticia para el usuario
        // ===================================================
        [HttpPost("{id}/noticias")]
        public async Task<IActionResult> AddNoticiaToUser(
            [FromRoute] string id,
            [FromBody] NoticiaCreateDto dto)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                var noticia = new Noticia(dto.Titulo, dto.Contenido)
                {
                    ImagenUrl = dto.ImagenUrl,
                    AutorId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                bool added = await _userService.AddNoticiaToUserAsync(user.Id, noticia);
                if (!added)
                    return _apiResponseService.BadRequestResponse("No se pudo crear la noticia");

                return _apiResponseService.OkResponse(noticia, "Noticia creada correctamente");
            });
        }

        // ===================================================
        // 🔹 DELETE: /user/{id}/noticias/{noticiaId}
        // Elimina una noticia (solo si pertenece al usuario)
        // ===================================================
        [HttpDelete("{id}/noticias/{noticiaId}")]
        public async Task<IActionResult> RemoveNoticiaFromUser(
            [FromRoute] string id,
            [FromRoute] int noticiaId)
        {
            return await _apiResponseService.Execute(async () =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return _apiResponseService.BadRequestResponse("El identificador no puede estar vacío");

                var user = await _userService.GetUserByIdOrEmailAsync(id);
                if (user == null)
                    return _apiResponseService.NotFoundResponse("Usuario no encontrado");

                bool removed = await _userService.RemoveNoticiaFromUserAsync(user.Id, noticiaId);
                if (!removed)
                    return _apiResponseService.NotFoundResponse("Noticia no encontrada o no pertenece al usuario");

                return _apiResponseService.OkResponse(null, "Noticia eliminada correctamente");
            });
        }
        /***********************************************************************************************************/
        [HttpGet("avatar")]
        public IActionResult GetAvatar()
        {
            try
            {
                // Ruta: assets/avatar/avatar.png
                var avatarPath = Path.Combine(_environment.ContentRootPath, "assets", "avatar", "avatar.png");

                if (!System.IO.File.Exists(avatarPath))
                {
                    return NotFound(new { message = "Image not found" });
                }

                // Detectar tipo MIME
                var contentType = "image/png";

                // Leer archivo y devolver
                var fileStream = new FileStream(avatarPath, FileMode.Open, FileAccess.Read);
                return File(fileStream, contentType);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error loading image" });
            }
        }
        // ===================================================
        // 🔹 GET: /api/image/{id}
        // Sirve una imagen por ID: 123 → busca 123.jpg, 123.png, etc.
        // ===================================================
        [HttpGet("image/{id}")]
        public IActionResult GetImageById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest(new { message = "Invalid image ID" });

                // Sanitizar ID
                if (id.Contains("..") || id.Contains("/") || id.Contains("\\"))
                    return BadRequest(new { message = "Invalid characters in ID" });

                // Carpetas y formatos a buscar
                var imagesFolder = Path.Combine(_environment.ContentRootPath, "assets", "images");
                var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                foreach (var ext in supportedExtensions)
                {
                    var imagePath = Path.Combine(imagesFolder, $"{id}{ext}");
                    if (System.IO.File.Exists(imagePath))
                    {
                        var contentType = GetContentType(ext);
                        var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                        return File(fileStream, contentType);
                    }
                }

                return NotFound(new { message = "Image not found" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error loading image" });
            }
        }

        // Ayuda: mapea extensión a MIME type
        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
        /***********************************************************************************************************/
    }

}