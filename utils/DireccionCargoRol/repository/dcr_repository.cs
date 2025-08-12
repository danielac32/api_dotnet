using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.model;


namespace backend_ont_2.DCR
{

    public class DireccionRepository
    {
        private readonly AppDbContext _context;
        public DireccionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Direccion>> getAll()
        {
            return await _context.Direcciones.ToListAsync();
        }

        public async Task<bool> create(Direccion dir)
        {
            _context.Direcciones.Add(dir);
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
        public async Task<bool> update(Direccion dir)
        {
            _context.Direcciones.Update(dir);
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
        public async Task<bool> delete(int id)
        {
            var dir = await _context.Direcciones.FindAsync(id);
            if (dir == null) return false;
            _context.Direcciones.Remove(dir);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Direccion> getById(int id)
        {
            var dir = await _context.Direcciones.FindAsync(id);
            return dir;
        }
        public async Task<bool> getByName(string name)
        {
            var dir = await _context.Direcciones.FirstOrDefaultAsync(p => p.Name == name);
            if (dir == null) return false;
            return true;
        }
    }
    /****************************************************************************/
    public class CargoRepository
    {
        private readonly AppDbContext _context;
        public CargoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cargo>> getAll()
        {
            return await _context.Cargos.ToListAsync();
        }

        public async Task<bool> create(Cargo cargo)
        {
            _context.Cargos.Add(cargo);
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
        public async Task<bool> update(Cargo cargo)
        {
            _context.Cargos.Update(cargo);
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
        public async Task<bool> delete(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return false;
            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Cargo> getById(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            return cargo;
        }

        public async Task<bool> getByName(string name)
        {
            var cargo = await _context.Cargos.FirstOrDefaultAsync(p => p.Name == name);
            if (cargo == null) return false;
            return true;
        }
    }
    /**************************************************************************************************/
    public class RoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> getAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<bool> create(Role rol)
        {
            _context.Roles.Add(rol);
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
        public async Task<bool> update(Role rol)
        {
            _context.Roles.Update(rol);
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
        public async Task<bool> delete(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;
            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Role> getById(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            return rol;
        }

        public async Task<bool> getByName(string name)
        {
            var rol = await _context.Roles.FirstOrDefaultAsync(p => p.Name == name);
            if (rol == null) return false;
            return true;
        }
    }
}