using Microsoft.AspNetCore.Mvc;
using System.Net;

/*
namespace backend_ont_2.shared.apiResponse
{
   public class ApiResponseService 
    {
        public class ApiResponse<T>
        {
            public int StatusCode { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
            public object Errors { get; set; }
        }

    // Clase de servicio para manejar respuestas
  
        public IActionResult Success<T>(T data = default, string message = null)
        {
            var response = new ApiResponse<T>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Success = true,
                Message = message ?? "Operación exitosa",
                Data = data
            };
            return new OkObjectResult(response);
        }

        public IActionResult Created<T>(T data = default, string uri = null, string message = null)
        {
            var response = new ApiResponse<T>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Success = true,
                Message = message ?? "Recurso creado exitosamente",
                Data = data
            };

            return !string.IsNullOrEmpty(uri) 
                ? new CreatedResult(uri, response) 
                : new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Created };
        }

        public IActionResult NoContent(string message = null)
        {
            var response = new ApiResponse<object>
            {
                StatusCode = (int)HttpStatusCode.NoContent,
                Success = true,
                Message = message ?? "Operación exitosa sin contenido"
            };
            return new NoContentResult();
        }

        public IActionResult Error(HttpStatusCode statusCode, string errorMessage, object details = null)
        {
            var response = new ApiResponse<object>
            {
                StatusCode = (int)statusCode,
                Success = false,
                Message = errorMessage,
                Errors = details
            };

            return statusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
                HttpStatusCode.Forbidden => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Forbidden },
                HttpStatusCode.NotFound => new NotFoundObjectResult(response),
                HttpStatusCode.Conflict => new ConflictObjectResult(response),
                HttpStatusCode.UnprocessableEntity => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.UnprocessableEntity },
                _ => new ObjectResult(response) { StatusCode = (int)statusCode }
            };
        }

        public async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (ValidationException ex)
            {
                return Error(HttpStatusCode.BadRequest, "Error de validación", ex.Errors);
            }
            catch (NotFoundException ex)
            {
                return Error(HttpStatusCode.NotFound, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Error(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (ForbiddenAccessException)
            {
                return Error(HttpStatusCode.Forbidden, "No tiene permisos para esta acción");
            }
            catch (ConflictException ex)
            {
                return Error(HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Error(HttpStatusCode.InternalServerError, "Error interno del servidor");
            }
        }
    }
    
}
*/

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