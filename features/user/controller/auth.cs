 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.features.user.dto;
using backend_ont_2.model;
using backend_ont_2.features.user.service.auth;
//using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace backend_ont_2.features.user.controller.auth
{

    [ApiController]
    [Route("auth/")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        //private readonly ApiResponseService _apiResponseService;

        public AuthController(AuthService authService/*, ApiResponseService apiResponseService*/)
        {
            _authService = authService;
           // _apiResponseService = apiResponseService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register(UserCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(createDto);

            if (!result.Success)
                return Conflict(new { message = result.Message }); // 409 si ya existe

            return Ok(result); // 200 OK con { Success, User, Token, Message }
            /*return await _apiResponseService.ExecuteAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return _apiResponseService.Error(HttpStatusCode.BadRequest, "Validación fallida", ModelState);
                }

                var result = await _authService.RegisterAsync(createDto);

                if (!result.Success)
                {
                    return _apiResponseService.Error(HttpStatusCode.Conflict, result.Message);
                }

                return _apiResponseService.Success(result.UserDto);
            });*/
        }



        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(result);
        }



    }
}