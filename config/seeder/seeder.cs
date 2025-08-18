using Microsoft.EntityFrameworkCore;
using backend_ont_2.data;
using backend_ont_2.model;
using BCrypt.Net;
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



            /*List<string> names = new List<string>
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
            };*/

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

            /*for (int i = 0; i < names.Count; i++)
            {
                var user = new User(
                    email: $"user{i + 1}@example.com",
                    password : BCrypt.Net.BCrypt.HashPassword("123456"), 
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
            }*/

            List<User> users = new List<User>
            {
                 
                //******************************************************/
                new User(
                    email: "darchi@gmail.com",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "darchi darchi",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370296"),
                    updatedAt: DateTime.Parse("2025-06-23T12:55:05.125")
                ),
                new User(
                    email: "adrian@gmail.com",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "adrian adrian",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Coordinador,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370298"),
                    updatedAt: DateTime.Parse("2025-06-27T10:31:05.880")
                ),
                new User(
                    email: "crnoguera@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "carlos noguera",
                    department: AppStrings.DgTecnologiaInformacion,
                    role: AppStrings.SuperAdmin,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370302"),
                    updatedAt: DateTime.Parse("2025-07-02T11:43:25.884")
                ),
                new User(
                    email: "nelson@gmail.com",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Nelson Arenas",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Coordinador,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370306"),
                    updatedAt: DateTime.Parse("2025-07-02T13:42:20.767")
                ),
                new User(
                    email: "kalmanza@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Keila Almanza",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370307"),
                    updatedAt: DateTime.Parse("2025-07-02T14:04:11.669")
                ),
                new User(
                    email: "dsoto@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Darchy Soto",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370311"),
                    updatedAt: DateTime.Parse("2025-07-02T14:10:41.369")
                ),
                new User(
                    email: "cmadriz@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Carlos Madriz",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.DirectorGeneral,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370315"),
                    updatedAt: DateTime.Parse("2025-07-02T15:58:40.840")
                ),
                new User(
                    email: "hcolmenarez@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Henry Colmenarez",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.DirectorLinea,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370319"),
                    updatedAt: DateTime.Parse("2025-07-02T15:28:14.538")
                ),
                new User(
                    email: "lsanchez@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Luisa Sanchez",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370321"),
                    updatedAt: DateTime.Parse("2025-07-02T15:29:31.102")
                ),
                new User(
                    email: "dnleon@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Duaving Leon",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370329"),
                    updatedAt: DateTime.Parse("2025-07-02T15:30:59.814")
                ),
                new User(
                    email: "mbarreto@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Maria Barreto",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370333"),
                    updatedAt: DateTime.Parse("2025-07-02T15:32:21.321")
                ),
                new User(
                    email: "vdiaz@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "Victor Diaz",
                    department: AppStrings.DgPlanificacionAnalisisFinanciero,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370334"),
                    updatedAt: DateTime.Parse("2025-07-02T15:33:05.136")
                ),
                new User(
                    email: "mcardenas@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "moises cardenas",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370339"),
                    updatedAt: DateTime.Parse("2025-07-04T10:15:58.213")
                ),
                new User(
                    email: "ycastillo@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "yasmin castillo",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370343"),
                    updatedAt: DateTime.Parse("2025-07-04T10:16:52.450")
                ),
                new User(
                    email: "emoreno@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "eilen moreno",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370347"),
                    updatedAt: DateTime.Parse("2025-07-04T10:17:24.989")
                ),
                new User(
                    email: "aegreso@gmail.com",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "alberth egreso",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370350"),
                    updatedAt: DateTime.Parse("2025-07-07T09:05:03.432")
                ),
                new User(
                    email: "arodriguez@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "adrián rodriguez",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Coordinador,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370352"),
                    updatedAt: DateTime.Parse("2025-07-07T10:14:14.608")
                ),
                new User(
                    email: "gmarcano@ont.gob.ve",
                    password: BCrypt.Net.BCrypt.HashPassword("123456"),
                    name: "grecia marcano",
                    department: AppStrings.DgEgreso,
                    role: AppStrings.User,
                    position: AppStrings.Analista,
                    createdAt: DateTime.Parse("2025-08-18T10:55:09.370356"),
                    updatedAt: DateTime.Parse("2025-07-07T10:15:16.374")
                )
            };

            foreach (var user in users)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                foreach (var section in sections)
                {
                    var permission = new Permission
                    {
                        UserId = user.Id,
                        Section = section,
                        CanCreate = user.Role == AppStrings.SuperAdmin,
                        CanEdit = user.Role == AppStrings.SuperAdmin,
                        CanDelete = user.Role == AppStrings.SuperAdmin,
                        CanPublish = user.Role == AppStrings.SuperAdmin,
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
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),//Password = "Admin123",
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
                Password = BCrypt.Net.BCrypt.HashPassword("123456"),//Password = "123456",
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
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),//
                Name = "Editor General",
                Role = AppStrings.Editor,
                Department = "DGComunicacion", // Asegúrate que exista o cambia a uno válido
                IsActive = true,
                Position = AppStrings.Analista,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.AddRange(superAdmin, superAdmin2, editor);
            await _context.SaveChangesAsync(); // Guardar para obtener IDs
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
            await _context.SaveChangesAsync();

            // 7. Crear alcaldía
            var alcaldia1 = new Alcaldia("Alcaldía Central", 500, 600, 700)
            {
                AutorId = superAdmin.Id
            };

            _context.Alcaldias.Add(alcaldia1);
            await _context.SaveChangesAsync();

            // 8. Crear programación financiera y meses
            var programacion1 = new ProgramacionFinanciera("Presupuesto Anual")
            {
                Descripcion = "Presupuesto para el año fiscal 2023",
                AutorId = superAdmin.Id
            };

            _context.ProgramacionesFinancieras.Add(programacion1);
            await _context.SaveChangesAsync(); // Para obtener el ID

            var mes1 = new Mes("Enero", 1000000, "PRESUPUESTO_INICIAL")
            {
                ProgramacionFinancieraId = programacion1.Id
            };

            var mes2 = new Mes("Febrero", 950000, "PRESUPUESTO_INICIAL")
            {
                ProgramacionFinancieraId = programacion1.Id
            };

            _context.Meses.AddRange(mes1, mes2);
            await _context.SaveChangesAsync();

            // 9. Resumen de gestión
            var resumen1 = new ResumenGestion("Informe Trimestral", "Resultados del primer trimestre", "https://example.com/informe.jpg")
            {
                AutorId = superAdmin.Id
            };

            _context.ResumenesGestion.Add(resumen1);
            await _context.SaveChangesAsync();

            // 10. Noticia
            var noticia1 = new Noticia("Nuevas iniciativas gubernamentales", "El gobierno anuncia nuevas medidas económicas...")
            {
                AutorId = editor.Id
            };

            _context.Noticias.Add(noticia1);
            await _context.SaveChangesAsync();

            Console.WriteLine("Seeder completado. Datos básicos creados.");
            return true;
        }
    }
}