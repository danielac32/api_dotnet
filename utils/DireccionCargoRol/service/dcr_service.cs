

// Services/DireccionCargoRolService.cs
using backend_ont_2.DCR;
using backend_ont_2.model;
 

namespace backend_ont_2.DCR.Services
{
    public class DireccionCargoRolService
    {
        private readonly DireccionRepository _direccionRepo;
        private readonly CargoRepository _cargoRepo;
        private readonly RoleRepository _roleRepo;

        public DireccionCargoRolService(
            DireccionRepository direccionRepo,
            CargoRepository cargoRepo,
            RoleRepository roleRepo)
        {
            _direccionRepo = direccionRepo;
            _cargoRepo = cargoRepo;
            _roleRepo = roleRepo;
        }

        // ===================================
        // 🔹 Métodos para DIRECCIÓN
        // ===================================

        public async Task<List<Direccion>> GetAllDirecciones() =>
            await _direccionRepo.getAll();

        public async Task<Direccion?> GetDireccionById(int id) =>
            await _direccionRepo.getById(id);

        public async Task<bool> CreateDireccion(Direccion direccion) =>
            await _direccionRepo.create(direccion);

        public async Task<bool> UpdateDireccion(Direccion direccion) =>
            await _direccionRepo.update(direccion);

        public async Task<bool> DeleteDireccion(int id) =>
            await _direccionRepo.delete(id);

        public async Task<bool> DireccionExists(string name) =>
            await _direccionRepo.getByName(name);

        // ===================================
        // 🔹 Métodos para CARGO
        // ===================================

        public async Task<List<Cargo>> GetAllCargos() =>
            await _cargoRepo.getAll();

        public async Task<Cargo?> GetCargoById(int id) =>
            await _cargoRepo.getById(id);

        public async Task<bool> CreateCargo(Cargo cargo) =>
            await _cargoRepo.create(cargo);

        public async Task<bool> UpdateCargo(Cargo cargo) =>
            await _cargoRepo.update(cargo);

        public async Task<bool> DeleteCargo(int id) =>
            await _cargoRepo.delete(id);

        public async Task<bool> CargoExists(string name) =>
            await _cargoRepo.getByName(name);

        // ===================================
        // 🔹 Métodos para ROLE
        // ===================================

        public async Task<List<Role>> GetAllRoles() =>
            await _roleRepo.getAll();

        public async Task<Role?> GetRoleById(int id) =>
            await _roleRepo.getById(id);

        public async Task<bool> CreateRole(Role role) =>
            await _roleRepo.create(role);

        public async Task<bool> UpdateRole(Role role) =>
            await _roleRepo.update(role);

        public async Task<bool> DeleteRole(int id) =>
            await _roleRepo.delete(id);

        public async Task<bool> RoleExists(string name) =>
            await _roleRepo.getByName(name);
    }
}