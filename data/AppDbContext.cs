
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.model;


namespace backend_ont_2.data   
{
    // Configuración del DbContext
    public class AppDbContext : DbContext
    {



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        
        public DbSet<Role> Roles { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MetaDato> MetaDatos { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<OrganismoGobernacion> OrganismosGobernacion { get; set; }
        public DbSet<Alcaldia> Alcaldias { get; set; }
        public DbSet<ProgramacionFinanciera> ProgramacionesFinancieras { get; set; }
        public DbSet<Mes> Meses { get; set; }
        public DbSet<ResumenGestion> ResumenesGestion { get; set; }
        public DbSet<Noticia> Noticias { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.MetaDato)
                .WithOne()
                .HasForeignKey<MetaDato>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de Permission
            modelBuilder.Entity<Permission>()
                .HasOne(p => p.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de OrganismoGobernacion
            modelBuilder.Entity<OrganismoGobernacion>()
                .HasOne(o => o.Autor)
                .WithMany(u => u.OrganismosGobernacion)
                .HasForeignKey(o => o.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Alcaldia
            modelBuilder.Entity<Alcaldia>()
                .HasOne(a => a.Autor)
                .WithMany(u => u.Alcaldias)
                .HasForeignKey(a => a.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de ProgramacionFinanciera
            modelBuilder.Entity<ProgramacionFinanciera>()
                .HasOne(p => p.Autor)
                .WithMany(u => u.ProgramacionesFinancieras)
                .HasForeignKey(p => p.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProgramacionFinanciera>()
                .HasMany(p => p.Meses)
                .WithOne(m => m.ProgramacionFinanciera)
                .HasForeignKey(m => m.ProgramacionFinancieraId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de ResumenGestion
            modelBuilder.Entity<ResumenGestion>()
                .HasOne(r => r.Autor)
                .WithMany(u => u.ResumenesGestion)
                .HasForeignKey(r => r.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Noticia
            modelBuilder.Entity<Noticia>()
                .HasOne(n => n.Autor)
                .WithMany(u => u.Noticias)
                .HasForeignKey(n => n.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración para actualizar automáticamente UpdatedAt
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetProperty("UpdatedAt") != null)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("UpdatedAt")
                        .HasDefaultValueSql("DATETIME('now')")
                        .ValueGeneratedOnAddOrUpdate();
                }

                if (entityType.ClrType.GetProperty("CreatedAt") != null)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .HasDefaultValueSql("DATETIME('now')");
                }
            }
        }
    }
}