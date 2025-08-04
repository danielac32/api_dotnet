 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.features.user.dto;
using backend_ont_2.model;
using backend_ont_2.features.user.service.auth;


namespace backend_ont_2.features.user.controller.auth
{

    [ApiController]
    [Route("auth/")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService )
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register(UserCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(createDto);

    
            return Conflict(new { message = result.Message });
    
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