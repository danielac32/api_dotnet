using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.model;
 
 
namespace backend_ont_2.features.user.repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        // Secciones como en Dart
        private readonly List<string> _sections = new()
        {
            AppStrings.Carrusel,
            AppStrings.Alcaldias,
            AppStrings.Organismos,
            AppStrings.Gobernacion,
            AppStrings.Noticias,
            AppStrings.ProgramacionFinanciera,
            AppStrings.ResumenGestion
            // Agrega más si es necesario
        };

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<List<User>> GetByActiveStatusAsync(string status)
        {
            return status.ToLower() switch
            {
                "active" => await _context.Users.Where(u => u.IsActive).ToListAsync(),
                "inactive" => await _context.Users.Where(u => !u.IsActive).ToListAsync(),
                "all" => await _context.Users.ToListAsync(),
                _ => throw new ArgumentException("Status inválido. Usa 'active', 'inactive' o 'all'")
            };
        }

        public async Task<List<User>> GetByRoleAsync(string role) =>
            await _context.Users.Where(u => u.Role == role).ToListAsync();

        public async Task<List<User>> GetByDepartmentAsync(string department) =>
            await _context.Users.Where(u => u.Department == department).ToListAsync();

        public async Task<int> CreateUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<int> CreateUserWithPermissionsAsync(User user)
        {
            var userId = await CreateUserAsync(user);

            var permissions = _sections.Select(s => new Permission()
            {
                Section = s,
                CanCreate = false,
                CanEdit = false,
                CanDelete = false,
                CanPublish = false,
                UserId = userId
            }).ToList();

            await _context.Permissions.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Gestión de permisos
        public async Task<List<Permission>> GetPermissionsByUserAsync(int userId)
        {
            return await _context.Permissions
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddPermissionToUserAsync(int userId, Permission permission)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            permission.UserId = userId;
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromUserAsync(int userId, int permissionId)
        {
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null || permission.UserId != userId) return false;

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Noticias
        public async Task<List<Noticia>> GetNoticiasByUserAsync(int userId)
        {
            return await _context.Noticias
                .Where(n => n.AutorId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddNoticiaToUserAsync(int userId, Noticia noticia)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            noticia.AutorId = userId;
            await _context.Noticias.AddAsync(noticia);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveNoticiaFromUserAsync(int userId, int noticiaId)
        {
            var noticia = await _context.Noticias.FindAsync(noticiaId);
            if (noticia == null || noticia.AutorId != userId) return false;

            _context.Noticias.Remove(noticia);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Organismos
        public async Task<List<OrganismoGobernacion>> GetOrganismosByUserAsync(int userId)
        {
            return await _context.OrganismosGobernacion
                .Where(o => o.AutorId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddOrganismoToUserAsync(int userId, OrganismoGobernacion organismo)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            organismo.AutorId = userId;
            await _context.OrganismosGobernacion.AddAsync(organismo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveOrganismoFromUserAsync(int userId, int organismoId)
        {
            var organismo = await _context.OrganismosGobernacion.FindAsync(organismoId);
            if (organismo == null || organismo.AutorId != userId) return false;

            _context.OrganismosGobernacion.Remove(organismo);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Programaciones
        public async Task<List<ProgramacionFinanciera>> GetProgramacionesByUserAsync(int userId)
        {
            return await _context.ProgramacionesFinancieras
                .Where(p => p.AutorId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddProgramacionToUserAsync(int userId, ProgramacionFinanciera programacion)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            programacion.AutorId = userId;
            await _context.ProgramacionesFinancieras.AddAsync(programacion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProgramacionFromUserAsync(int userId, int programacionId)
        {
            var programacion = await _context.ProgramacionesFinancieras.FindAsync(programacionId);
            if (programacion == null || programacion.AutorId != userId) return false;

            _context.ProgramacionesFinancieras.Remove(programacion);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Resúmenes
        public async Task<List<ResumenGestion>> GetResumenesByUserAsync(int userId)
        {
            return await _context.ResumenesGestion
                .Where(r => r.AutorId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddResumenToUserAsync(int userId, ResumenGestion resumen)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            resumen.AutorId = userId;
            await _context.ResumenesGestion.AddAsync(resumen);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveResumenFromUserAsync(int userId, int resumenId)
        {
            var resumen = await _context.ResumenesGestion.FindAsync(resumenId);
            if (resumen == null || resumen.AutorId != userId) return false;

            _context.ResumenesGestion.Remove(resumen);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}