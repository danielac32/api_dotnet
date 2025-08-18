using backend_ont_2.features.user.repositories;
using backend_ont_2.model;


namespace backend_ont_2.features.user.service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdOrEmailAsync(string identifier)
        {
            return await _userRepository.GetUserByIdOrEmailAsync(identifier);
        }
        public async Task<List<User>> getUserByFilter(string? role, string? department, string? status)
        {
            if (!string.IsNullOrEmpty(role))
                return await _userRepository.GetByRoleAsync(role);

            if (!string.IsNullOrEmpty(department))
                return await _userRepository.GetByDepartmentAsync(department);

            if (!string.IsNullOrEmpty(status))
                return await _userRepository.GetByActiveStatusAsync(status);

            return await _userRepository.GetAllAsync();
        }

        public async Task<List<User>> GetAllAsync() =>
            await _userRepository.GetAllAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _userRepository.GetByIdAsync(id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userRepository.GetByEmailAsync(email);

        public async Task<List<User>> GetByActiveStatusAsync(string status) =>
            await _userRepository.GetByActiveStatusAsync(status);

        public async Task<List<User>> GetByRoleAsync(string role) =>
            await _userRepository.GetByRoleAsync(role);

        public async Task<int> CreateUserWithPermissionsAsync(User user)
        {
            // Aquí puedes agregar lógica: encriptar contraseña, validar email, etc.
            return await _userRepository.CreateUserWithPermissionsAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user) =>
            await _userRepository.UpdateUserAsync(user);

        public async Task<bool> DeleteUserAsync(int userId) =>
            await _userRepository.DeleteUserAsync(userId);
        /********************************************************************************************************/
        public async Task<List<Permission>> GetPermissionsByUserAsync(int userId) =>
            await _userRepository.GetPermissionsByUserAsync(userId);

        public async Task<Permission?> GetPermissionByUserAndSection(int userId, string section)
        {
            return await _userRepository.GetPermissionByUserAndSection(userId,section);
        }
        public async Task<bool> UpdatePermissionAsync(Permission permission)
        {
            return await _userRepository.UpdatePermissionAsync(permission);
        }

        public async Task<bool> AddPermissionToUserAsync(int userId, Permission permission) =>
            await _userRepository.AddPermissionToUserAsync(userId, permission);

        public async Task<Permission> GetPermissionById(int id) =>
          await _userRepository.GetPermissionById(id);

        public async Task<bool> RemovePermissionFromUserAsync(int userId, int permissionId) =>
            await _userRepository.RemovePermissionFromUserAsync(userId, permissionId);

        /********************************************************************************************************/
        public async Task<List<OrganismoGobernacion>> GetOrganismosByUserAsync(int userId)
        {
            return await _userRepository.GetOrganismosByUserAsync(userId);
        }

        public async Task<bool> AddOrganismoToUserAsync(int userId, OrganismoGobernacion organismo)
        {
            return await _userRepository.AddOrganismoToUserAsync(userId, organismo);
        }

        public async Task<bool> RemoveOrganismoFromUserAsync(int userId, int organismoId)
        {
            return await _userRepository.RemoveOrganismoFromUserAsync(userId, organismoId);
        }
        /********************************************************************************************************/
        public async Task<List<ProgramacionFinanciera>> GetProgramacionesByUserAsync(int userId)
        {
            return await _userRepository.GetProgramacionesByUserAsync(userId);
        }

        public async Task<bool> AddProgramacionToUserAsync(int userId, ProgramacionFinanciera programacion)
        {
            return await _userRepository.AddProgramacionToUserAsync(userId,programacion);
        }

        public async Task<bool> RemoveProgramacionFromUserAsync(int userId, int programacionId)
        {
           return await _userRepository.RemoveProgramacionFromUserAsync(userId,programacionId);
        }

        /********************************************************************************************************/
        public async Task<List<ResumenGestion>> GetResumenesByUserAsync(int userId)
        {
            return await _userRepository.GetResumenesByUserAsync(userId);
        }

        public async Task<bool> AddResumenToUserAsync(int userId, ResumenGestion resumen)
        {
            return await _userRepository.AddResumenToUserAsync(userId,resumen);
        }

        public async Task<bool> RemoveResumenFromUserAsync(int userId, int resumenId)
        {
            return await _userRepository.RemoveResumenFromUserAsync(userId,resumenId);
        }
        /********************************************************************************************************/
        public async Task<List<Noticia>> GetNoticiasByUserAsync(int userId)
        {
            return await _userRepository.GetNoticiasByUserAsync(userId);
        }

        public async Task<bool> AddNoticiaToUserAsync(int userId, Noticia noticia)
        {
            return await _userRepository.AddNoticiaToUserAsync(userId,noticia);
        }

        public async Task<bool> RemoveNoticiaFromUserAsync(int userId, int noticiaId)
        {
            return await _userRepository.RemoveNoticiaFromUserAsync(userId,noticiaId);
        }
        /********************************************************************************************************/
    }
}