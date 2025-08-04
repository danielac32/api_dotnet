// features/controller/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.features.user.dto;
using backend_ont_2.model;
/*
namespace backend_ont_2.features.user.controller
{
    [ApiController]
    [Route("user/")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        



        // 🔹 GET: api/user - Listar todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Department = u.Department,
                    Position = u.Position,
                    IsActive = u.IsActive,
                    ProfileImage = u.ProfileImage,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        // 🔹 GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var userDto = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Department = u.Department,
                    Position = u.Position,
                    IsActive = u.IsActive,
                    ProfileImage = u.ProfileImage,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (userDto == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            return Ok(userDto);
        }

        // 🔹 POST: api/user - Crear usuario
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Users.Any(u => u.Email == createDto.Email))
            {
                return BadRequest(new { message = "El email ya está registrado." });
            }

            // ✅ Usa el constructor con parámetros
            var user = new User(
                email: createDto.Email,
                password: createDto.Password, // ⚠️ En producción: hashear
                name: createDto.Name,
                department: createDto.Department,
                role: createDto.Role,
                position: createDto.Position,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow
            );

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Department = user.Department,
                Position = user.Position,
                IsActive = user.IsActive,
                ProfileImage = user.ProfileImage,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // 🔹 PUT: api/user/5 - Actualizar
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            user.CopyWith(
                email: updateDto.Email,
                name: updateDto.Name,
                role: updateDto.Role,
                department: updateDto.Department,
                isActive: updateDto.IsActive,
                position: updateDto.Position,
                profileImage: updateDto.ProfileImage
            );

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // 🔹 DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            user.CopyWith(isActive: false); // Borrado lógico
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}*/