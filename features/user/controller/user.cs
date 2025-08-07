 
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
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return await _apiResponseService.Execute(async () =>
            {
                User user = await _userService.GetByIdAsync(id);
                return _apiResponseService.OkResponse(data: user);
            });
        }

        // PATCH: /user/{id}
        [HttpPatch("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateById(
            [FromRoute] int id,
            [FromBody] UserUpdateDto updateDto) // Puedes usar dynamic o un DTO real
        {

            return await _apiResponseService.Execute(async () =>
           {
               if (id <= 0) return _apiResponseService.BadRequestResponse("ID inválido");
               User user = await _userService.GetByIdAsync(id);
               if (user == null)
               {
                   return _apiResponseService.NotFoundResponse("El usuario no existe");
               }
                
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

               return _apiResponseService.OkResponse(data:updatedUser);
           });
        }

        // DELETE: /user/{id}
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            return await _apiResponseService.Execute(async () =>
           {
               if (id <= 0)
                   return BadRequest(new { error = "ID inválido" });

               var delete = _userService.DeleteUserAsync(id);
               return _apiResponseService.OkResponse(message:"Usuario eliminado");
            });
        }
    }

}