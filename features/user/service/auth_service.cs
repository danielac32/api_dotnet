using backend_ont_2.features.user.repositories;
using backend_ont_2.model;
using backend_ont_2.features.user.dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;


namespace backend_ont_2.features.user.service.auth
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        public AuthService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        public async Task<AuthResponseDto> RegisterAsync(UserCreateDto createDto)
        {
            var existingUser = await _userService.GetByEmailAsync(createDto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Ya existe un usuario con este correo electrónico."
                };
            }

            var user = new User()
            {
                Email = createDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(createDto.Password),
                Name = createDto.Name,
                Role = createDto.Role,
                Department = createDto.Department,
                Position = createDto.Position,
                ProfileImage = createDto.ProfileImage,
                IsActive = createDto.IsActive ?? false //true
            };

            var userId = await _userService.CreateUserWithPermissionsAsync(user);

            if (userId > 0)
            {
                var userDto = new UserDto
                {
                    Id = userId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Department = user.Department,
                    Position = user.Position,
                    ProfileImage = user.ProfileImage
                };

                var token = GenerateJwtToken(user.Email, user.Role, userId);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Usuario registrado exitosamente",
                    User = userDto,
                    Token = token
                };
            }

            return new AuthResponseDto
            {
                Success = false,
                Message = "Error al registrar el usuario."
            };
        }

        public async Task<AuthResponseDto> LoginAsync(string email, string password)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Credenciales inválidas."
                };
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Department = user.Department,
                Position = user.Position,
                ProfileImage = user.ProfileImage
            };

            var token = GenerateJwtToken(user.Email, user.Role, user.Id);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Inicio de sesión exitoso",
                User = userDto,
                Token = token
            };
        }

        private string GenerateJwtToken(string email, string role, int userId)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}