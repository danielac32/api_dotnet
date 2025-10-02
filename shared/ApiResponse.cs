using Microsoft.AspNetCore.Mvc;
using System;


namespace backend_ont_2.shared.apiResponse
{
    public class ApiResponseService
    {
        public async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (ArgumentException ex)
            {
               return new BadRequestObjectResult(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");

                return new ObjectResult(new
                {
                    Success = false,
                    Error = "Ocurrió un error interno",
                    Details = ex.Message
                })
                {
                    StatusCode = 500
                };
            }
        }

        public IActionResult OkResponse(object data = null, string message = "Operación exitosa")
        {
            return new OkObjectResult(new
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        public IActionResult CreatedResponse(string actionName, object routeValues, object data)
        {
            return new CreatedAtActionResult(actionName, null, routeValues, data);
        }

        public IActionResult NoContentResponse()
        {
            return new NoContentResult();
        }

        public IActionResult BadRequestResponse(string error)
        {
            return new BadRequestObjectResult(new
            {
                //Success = false,
                Error = error
            });
        }

        /*public IActionResult BadRequestResponse(ModelStateDictionary modelState)
        {
            return new BadRequestObjectResult(modelState);
        }*/

        public IActionResult UnauthorizedResponse()
        {
            return new UnauthorizedResult();
        }

        public IActionResult ForbidResponse()
        {
            return new ForbidResult();
        }

        public IActionResult NotFoundResponse(string message)
        {
            return new NotFoundObjectResult(new
            {
                //Success = false,
                Error = message
            });
        }

        public IActionResult ConflictResponse(string message)
        {
            return new ConflictObjectResult(new
            {
                //Success = false,
                Error = message
            });
        }
    }
}
 

/*

[HttpGet("{id}")]
public async Task<IActionResult> GetUser(int id)
{
    return await _apiResponse.ExecuteAsync(async () =>
    {
        var user = await _userService.GetUserAsync(id);
        return _apiResponse.Success(user);
    });
}

/*

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IApiResponseService _apiResponse;

    public UsersController(IUserService userService, IApiResponseService apiResponse)
    {
        _userService = userService;
        _apiResponse = apiResponse;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return await _apiResponse.ExecuteAsync(async () =>
        {
            var user = await _userService.GetByIdAsync(id);
            return _apiResponse.Success(user);
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto userDto)
    {
        return await _apiResponse.ExecuteAsync(async () =>
        {
            var createdUser = await _userService.CreateAsync(userDto);
            return _apiResponse.Created(createdUser, $"/api/users/{createdUser.Id}");
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _apiResponse.ExecuteAsync(async () =>
        {
            await _userService.DeleteAsync(id);
            return _apiResponse.NoContent();
        });
    }
}
*/


/*

// 200 OK
return Ok(new { message = "Operación exitosa", data = items });

// 201 Created
return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);

// 204 No Content
return NoContent();

// 400 Bad Request
return BadRequest(new { error = "El campo 'Email' es requerido" });

// 401 Unauthorized
return Unauthorized();

// 403 Forbid
return Forbid();

// 404 Not Found
return NotFound(new { error = "Usuario no encontrado" });

// 409 Conflict
return Conflict(new { error = "El correo ya está registrado" });
*/