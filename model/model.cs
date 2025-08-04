using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace backend_ont_2.model  // o namespace backend_ont.data (dependiendo de la capitalización)
{
        public class Role
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            public Role(string name) => Name = name;
            public Role CopyWith(string? name = null)
            {
                if (name != null) Name = name;
                return this;
            }
        }

        public class Cargo
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            public Cargo(string name) => Name = name;
            public Cargo CopyWith(string? name = null)
            {
                if (name != null) Name = name;
                return this;
            }
        }

        public class Direccion
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            public Direccion(string name) => Name = name;
            public Direccion CopyWith(string? name = null)
            {
                if (name != null) Name = name;
                return this;
            }
        }
    
    public class MetaDato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(100)]
        public string? CentroVotacion { get; set; }

        [StringLength(20)]
        public string? Cedula { get; set; }

        [StringLength(50)]
        public string? Estado { get; set; }

        [StringLength(50)]
        public string? Municipio { get; set; }

        [StringLength(50)]
        public string? Parroquia { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public MetaDato(int userId) => UserId = userId;
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "USER";

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(255)]
        public string? ProfileImage { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = "Analista";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relaciones
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        public ICollection<OrganismoGobernacion> OrganismosGobernacion { get; set; } = new List<OrganismoGobernacion>();
        public ICollection<Alcaldia> Alcaldias { get; set; } = new List<Alcaldia>();
        public ICollection<ProgramacionFinanciera> ProgramacionesFinancieras { get; set; } = new List<ProgramacionFinanciera>();
        public ICollection<ResumenGestion> ResumenesGestion { get; set; } = new List<ResumenGestion>();
        public ICollection<Noticia> Noticias { get; set; } = new List<Noticia>();
        public MetaDato? MetaDato { get; set; }

// ✅ Constructor sin parámetros (para EF Core)
        public User()
        {
            // EF Core lo usa para crear instancias
            // No hacer nada aquí, las propiedades se asignan después
        }
        public User(string email, string password, string name, string department, string role, string position, DateTime createdAt, DateTime updatedAt){
            Email = email;
            Password = password;
            Name = name;
            Department = department;
            Role = role;
            Position = position;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        /*public User(string email, string password, string name, string department,string role)
        {
            Email = email;
            Password = password;
            Name = name;
            Department = department;
            Role = role;
        }*/

        public User CopyWith(string? email = null, string? password = null, string? name = null, 
                        string? role = null, string? department = null, bool? isActive = null,
                        string? position = null, string? profileImage = null)
        {
            if (email != null) Email = email;
            if (password != null) Password = password;
            if (name != null) Name = name;
            if (role != null) Role = role;
            if (department != null) Department = department;
            if (isActive != null) IsActive = isActive.Value;
            if (position != null) Position = position;
            if (profileImage != null) ProfileImage = profileImage;
            
            UpdatedAt = DateTime.UtcNow;
            return this;
        }
    }

    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [StringLength(50)]
        public string Section { get; set; }

        public bool CanCreate { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanPublish { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Permission() { }

        //public Permission(string section) => Section = section;

        public Permission CopyWith(string? section = null, bool? canCreate = null, 
                                bool? canEdit = null, bool? canDelete = null, 
                                bool? canPublish = null)
        {
            if (section != null) Section = section;
            if (canCreate != null) CanCreate = canCreate.Value;
            if (canEdit != null) CanEdit = canEdit.Value;
            if (canDelete != null) CanDelete = canDelete.Value;
            if (canPublish != null) CanPublish = canPublish.Value;
            
            UpdatedAt = DateTime.UtcNow;
            return this;
        }

        public override string ToString() => 
            $"{Id} - {Section} - {CanCreate} - {CanDelete} - {CanEdit} - {CanPublish}";
    }

    public class OrganismoGobernacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int Valor1 { get; set; }

        [Required]
        public int Valor2 { get; set; }

        [Required]
        public int Valor3 { get; set; }

        [Required]
        public int AutorId { get; set; }

        [ForeignKey("AutorId")]
        public User Autor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public OrganismoGobernacion(string nombre, int valor1, int valor2, int valor3)
        {
            Nombre = nombre;
            Valor1 = valor1;
            Valor2 = valor2;
            Valor3 = valor3;
        }
    }

    public class Alcaldia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int Valor1 { get; set; }

        [Required]
        public int Valor2 { get; set; }

        [Required]
        public int Valor3 { get; set; }

        [Required]
        public int AutorId { get; set; }

        [ForeignKey("AutorId")]
        public User Autor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Alcaldia(string nombre, int valor1, int valor2, int valor3)
        {
            Nombre = nombre;
            Valor1 = valor1;
            Valor2 = valor2;
            Valor3 = valor3;
        }
    }

    public class ProgramacionFinanciera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        public int AutorId { get; set; }

        [ForeignKey("AutorId")]
        public User Autor { get; set; }

        public ICollection<Mes> Meses { get; set; } = new List<Mes>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ProgramacionFinanciera(string titulo) => Titulo = titulo;
    }

    public class Mes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Required]
        public int ProgramacionFinancieraId { get; set; }

        [ForeignKey("ProgramacionFinancieraId")]
        public ProgramacionFinanciera ProgramacionFinanciera { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Mes(string nombre, double valor, string tipo)
        {
            Nombre = nombre;
            Valor = valor;
            Tipo = tipo;
        }
    }

    public class ResumenGestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(255)]
        public string ImagenUrl { get; set; }

        [Required]
        public int AutorId { get; set; }

        [ForeignKey("AutorId")]
        public User Autor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ResumenGestion(string titulo, string descripcion, string imagenUrl)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            ImagenUrl = imagenUrl;
        }
    }

    public class Noticia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Contenido { get; set; }

        [StringLength(255)]
        public string? ImagenUrl { get; set; }

        [Required]
        public int AutorId { get; set; }

        [ForeignKey("AutorId")]
        public User Autor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Noticia(string titulo, string contenido)
        {
            Titulo = titulo;
            Contenido = contenido;
        }
    }
}