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

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _userRepository.GetAllAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _userRepository.GetByIdAsync(id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userRepository.GetByEmailAsync(email);

        public async Task<IEnumerable<User>> GetByActiveStatusAsync(string status) =>
            await _userRepository.GetByActiveStatusAsync(status);

        public async Task<IEnumerable<User>> GetByRoleAsync(string role) =>
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

        public async Task<List<Permission>> GetPermissionsByUserAsync(int userId) =>
            await _userRepository.GetPermissionsByUserAsync(userId);

        public async Task<bool> AddPermissionToUserAsync(int userId, Permission permission) =>
            await _userRepository.AddPermissionToUserAsync(userId, permission);

        public async Task<bool> RemovePermissionFromUserAsync(int userId, int permissionId) =>
            await _userRepository.RemovePermissionFromUserAsync(userId, permissionId);
    }
}