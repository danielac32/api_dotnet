 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.features.user.dto;
using backend_ont_2.model;
using backend_ont_2.features.user.service.auth;
using backend_ont_2.shared.apiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace backend_ont_2.features.user.controller.auth
{

    [ApiController]
    [Route("auth/")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ApiResponseService _apiResponseService;

        public AuthController(AuthService authService, ApiResponseService apiResponseService)
        {
            _authService = authService;
            _apiResponseService = apiResponseService;
        }

        [HttpPost("register")]
        //[Authorize]
        public async Task<IActionResult> register(UserCreateDto createDto)
        {
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(createDto);

            if (!result.Success)
                return Conflict(new { message = result.Message }); // 409 si ya existe

            return Ok(result); // 200 OK con { Success, User, Token, Message }*/
            return await _apiResponseService.Execute(async () =>
            {
                if (!ModelState.IsValid)
                    return _apiResponseService.BadRequestResponse("Datos de entrada inválidos");

                var result = _authService.RegisterAsync(createDto).Result; // Usar .Result por el Execute

                if (!result.Success)
                    return _apiResponseService.ConflictResponse(result.Message);

                return _apiResponseService.OkResponse(result);
            });
        }



        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDto loginDto)
        {

            return await _apiResponseService.Execute(async () =>
            {
                if (!ModelState.IsValid)
                    return _apiResponseService.BadRequestResponse("Datos de entrada inválidos");

                var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

                if (!result.Success)
                    return Unauthorized(new { message = result.Message });

                return Ok(result);

            });
            /*if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

                    if (!result.Success)
                        return Unauthorized(new { message = result.Message });

                    return Ok(result);*/
            }



    }
}