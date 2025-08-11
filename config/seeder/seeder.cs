using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.model;

namespace backend_ont_2.seeder
{
    public class Seeder
    {
        private readonly AppDbContext _context;
        public Seeder(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> Run()
        {
            // Clear existing data
            _context.Roles.RemoveRange(_context.Roles);
            _context.Cargos.RemoveRange(_context.Cargos);
            _context.Direcciones.RemoveRange(_context.Direcciones);
            _context.Users.RemoveRange(_context.Users);
            _context.Permissions.RemoveRange(_context.Permissions);
            _context.OrganismosGobernacion.RemoveRange(_context.OrganismosGobernacion);
            _context.Alcaldias.RemoveRange(_context.Alcaldias);
            _context.ProgramacionesFinancieras.RemoveRange(_context.ProgramacionesFinancieras);
            _context.Meses.RemoveRange(_context.Meses);
            _context.ResumenesGestion.RemoveRange(_context.ResumenesGestion);
            _context.Noticias.RemoveRange(_context.Noticias);
            
            await _context.SaveChangesAsync();



            List<string> names = new List<string>
            {
                "Juan Perez", "Maria Lopez", "Carlos Ramirez", "Ana Gomez", "Luis Martinez",
                "Laura Hernandez", "Pedro Sanchez", "Sofia Torres", "Diego Flores", "Camila Rios",
                "Javier Mendoza", "Valeria Castro", "Fernando Silva", "Isabella Morales", "Ricardo Guzman",
                "Daniela Vargas", "Miguel Navarro", "Gabriela Ortega", "Andres Paredes", "Carolina Jimenez",
                "Alejandro Ruiz", "Natalia Rojas", "Oscar Acosta", "Paula Vega", "Victor Soto",
                "Lucia Mora", "Manuel Espinoza", "Carmen Alvarado", "Roberto Medina", "Elena Chavez",
                "Sebastian Ponce", "Monica Salgado", "Hugo Nunez", "Adriana Aguilar", "Julio Contreras",
                "Veronica Molina", "Raul Peña", "Patricia Leon", "Ernesto Bravo", "Martha Campos",
                "Alberto Zuniga", "Beatriz Pacheco", "Felipe Rosales", "Gloria Villanueva", "Rodrigo Galindo",
                "Irene Calderon", "Francisco Padilla", "Claudia Fuentes", "Arturo Velazquez"
            };
            
            // Lista de roles disponibles
            List<string> roles = new List<string>
            {
                AppStrings.SuperAdmin,
                AppStrings.DepartmentAdmin,
                AppStrings.Editor,
                AppStrings.Viewer,
                AppStrings.Guest,
                AppStrings.User,
                AppStrings.Admin,
            };

            // Lista de departamentos disponibles
            List<string> departments = new List<string>
            {
                AppStrings.DgAdministracion,
                AppStrings.DgEgreso,
                AppStrings.DgIngreso,
                AppStrings.DgCuentaUnica,
                AppStrings.DgTecnologiaInformacion,
                AppStrings.DgPlanificacionAnalisisFinanciero,
                AppStrings.DgRecursosHumanos,
                AppStrings.DgInversionesYValores,
                AppStrings.DgConsultoriaJuridica,
            };

            // Lista de cargos disponibles
            List<string> positions = new List<string>
            {
                AppStrings.Coordinador,
                AppStrings.DirectorGeneral,
                AppStrings.DirectorLinea,
                AppStrings.Asistente,
                AppStrings.Analista,
                AppStrings.Asesor,
                AppStrings.Consultor,
                AppStrings.Hp,
                AppStrings.Otro,
            };
            
            List<string> sections = new List<string>
            {
                AppStrings.Carrusel,
                AppStrings.Alcaldias,
                AppStrings.Organismos,
                AppStrings.Gobernacion,
                AppStrings.Noticias,
                AppStrings.ProgramacionFinanciera,
                AppStrings.ResumenGestion,
                // Agrega más secciones según sea necesario
            };

            for (int i = 0; i < names.Count; i++)
            {
                var user = new User(
                    email: $"user{i + 1}@example.com",
                    password: "123456",
                    name: names[i % names.Count],
                    department: departments[i % departments.Count],
                    role: roles[i % roles.Count],
                    position: positions[i % positions.Count],
                    createdAt: DateTime.UtcNow,
                    updatedAt: DateTime.UtcNow
                );

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                foreach (var section in sections)
                {
                    var permission = new Permission
                    {
                        UserId = user.Id,
                        Section = section,
                        CanCreate = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanPublish = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Permissions.Add(permission);
                    await _context.SaveChangesAsync();
                }
            }

            foreach (var rol in roles)
            {
                var _rol = new Role(rol);
                _context.Roles.AddRange(_rol);
                await _context.SaveChangesAsync();
            }

            foreach (var cargo in positions)
            {
                var _cargo = new Cargo(cargo);
                _context.Cargos.AddRange(_cargo);
               await _context.SaveChangesAsync();
            }

            foreach (var dir in departments)
            {
                var _dir = new Direccion(dir);
                _context.Direcciones.AddRange(_dir);
                await _context.SaveChangesAsync();
            }
            
            var superAdmin = new User
            {
                Email = "admin@example.com",
                Password = "Admin123",
                Name = "Administrador Principal",
                Role = AppStrings.SuperAdmin,
                Department = AppStrings.DgAdministracion,
                IsActive = true,
                Position = AppStrings.DirectorGeneral,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var superAdmin2 = new User
            {
                Email = "daniel@gmail.com",
                Password = "123456",
                Name = "daniel quintero",
                Role = AppStrings.SuperAdmin,
                Department = AppStrings.DgTecnologiaInformacion,
                IsActive = true,
                Position = AppStrings.Coordinador,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var editor = new User
            {
                Email = "editor@example.com",
                Password = "Editor123",
                Name = "Editor General",
                Role = AppStrings.Editor,
                Department = "DGComunicacion", // Asegúrate que exista o cambia a uno válido
                IsActive = true,
                Position = AppStrings.Analista,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.AddRange(superAdmin, superAdmin2, editor);
            await  _context.SaveChangesAsync(); // Guardar para obtener IDs
// 5. Permisos para usuarios especiales
            var specialPermissions = new List<Permission>();
            foreach (var section in sections)
            {
                specialPermissions.Add(new Permission
                {
                    UserId = superAdmin.Id,
                    Section = section,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CanPublish = false
                });

                specialPermissions.Add(new Permission
                {
                    UserId = superAdmin2.Id,
                    Section = section,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CanPublish = false
                });

                specialPermissions.Add(new Permission
                {
                    UserId = editor.Id,
                    Section = section,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CanPublish = false
                });
            }

            _context.Permissions.AddRange(specialPermissions);
            await _context.SaveChangesAsync();

            // 6. Crear organismos (con autor superAdmin)
            var organismo1 = new OrganismoGobernacion("Ministerio de Hacienda", 100, 200, 300)
            {
                AutorId = superAdmin.Id
            };

            var organismo2 = new OrganismoGobernacion("Ministerio de Hacienda", 100, 200, 300)
            {
                AutorId = superAdmin.Id
            };

            var organismo3 = new OrganismoGobernacion("Ministerio de Hacienda", 100, 200, 300)
            {
                AutorId = superAdmin.Id
            };

            _context.OrganismosGobernacion.AddRange(organismo1, organismo2, organismo3);
           await  _context.SaveChangesAsync();

            // 7. Crear alcaldía
            var alcaldia1 = new Alcaldia("Alcaldía Central", 500, 600, 700)
            {
                AutorId = superAdmin.Id
            };

            _context.Alcaldias.Add(alcaldia1);
           await  _context.SaveChangesAsync();

            // 8. Crear programación financiera y meses
            var programacion1 = new ProgramacionFinanciera("Presupuesto Anual")
            {
                Descripcion = "Presupuesto para el año fiscal 2023",
                AutorId = superAdmin.Id
            };

            _context.ProgramacionesFinancieras.Add(programacion1);
           await  _context.SaveChangesAsync(); // Para obtener el ID

            var mes1 = new Mes("Enero", 1000000, "PRESUPUESTO_INICIAL")
            {
                ProgramacionFinancieraId = programacion1.Id
            };

            var mes2 = new Mes("Febrero", 950000, "PRESUPUESTO_INICIAL")
            {
                ProgramacionFinancieraId = programacion1.Id
            };

            _context.Meses.AddRange(mes1, mes2);
           await  _context.SaveChangesAsync();

            // 9. Resumen de gestión
            var resumen1 = new ResumenGestion("Informe Trimestral", "Resultados del primer trimestre", "https://example.com/informe.jpg")
            {
                AutorId = superAdmin.Id
            };

            _context.ResumenesGestion.Add(resumen1);
           await  _context.SaveChangesAsync();

            // 10. Noticia
            var noticia1 = new Noticia("Nuevas iniciativas gubernamentales", "El gobierno anuncia nuevas medidas económicas...")
            {
                AutorId = editor.Id
            };

            _context.Noticias.Add(noticia1);
           await  _context.SaveChangesAsync();

            Console.WriteLine("Seeder completado. Datos básicos creados.");
            return true;
        }
    }
}