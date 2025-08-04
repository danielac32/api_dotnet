// features/controller/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.features.user.dto;
using backend_ont_2.model;
using backend_ont_2.features.user.service;
// controllers/ExampleController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_ont_2.features.user.controller.user
{
    [ApiController]
    [Route("user/")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

 // GET: /user/get  → sin auth
        [HttpGet("get")]
        public async Task<IActionResult>  List()
        {
            var users = new[]
            {
                new { id = 1, name = "Usuario Público", email = "publico@ejemplo.com" }
            };

            return Ok(new
            {
                success = true,
                users = users
            });
        }

        // GET: /user  → con auth
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListWithAuth()
        {
            var users = new[]
            {
                new { id = 2, name = "Usuario Auth", email = "auth@ejemplo.com" }
            };

            return Ok(new
            {
                success = true,
                users = users
            });
        }

        // GET: /user/filter?status=active
        [HttpGet("filter")]
        [Authorize]
        public async Task<IActionResult> ListFilter([FromQuery] string? status, [FromQuery] string? role)
        {
            // Simula que usas los query params
            return Ok(new
            {
                success = true,
                users = new[]
                {
                    new { filter = new { status, role }, data = "datos simulados" }
                }
            });
        }

        // GET: /user/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            // Aquí simularías validateGetIdMiddleware → id > 0
            if (id <= 0)
                return BadRequest(new { error = "ID inválido" });

            return Ok(new
            {
                success = true,
                user = new { id, name = "Usuario por ID", email = $"user{id}@ejemplo.com" }
            });
        }

        // PATCH: /user/{id}
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateById(
            [FromRoute] int id,
            [FromBody] object body) // Puedes usar dynamic o un DTO real
        {
            if (id <= 0)
                return BadRequest(new { error = "ID inválido" });

            // Simula que extraes del body
            // En Dart: dynamicRequest.call("name"), etc.
            // Aquí: asumes que el body tiene datos
            return Ok(new
            {
                success = true,
                message = "Usuario actualizado (simulado)",
                id,
                updatedData = body // Devuelve el body recibido como ejemplo
            });
        }

        // DELETE: /user/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { error = "ID inválido" });

            return Ok(new
            {
                success = true,
                message = "Usuario eliminado (simulado)",
                id
            });
        }



    }

}