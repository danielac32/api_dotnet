// Program.cs
using backend_ont_2.data; // Ajusta según tu namespace
using Microsoft.EntityFrameworkCore;
using backend_ont_2.features.user.repositories;
using backend_ont_2.features.user.service;
using backend_ont_2.features.user.service.auth;
using backend_ont_2.shared.apiResponse;
using backend_ont_2.seeder;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend_ont_2.DCR;
using backend_ont_2.DCR.Services;
using backend_ont_2.OracleDbProject;
using backend_ont_2.Middleware;
// ================================
// Parsear argumentos con formato: -clave valor
// ================================

var argsDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
for (int i = 0; i < args.Length; i++)
{
    if (args[i].StartsWith("-") && i + 1 < args.Length)
    {
        string _key = args[i].Substring(1); // quita el -
        string value = args[i + 1];
        argsDict[_key] = value;
        i++; // saltar el valor
    }
}

// ================================
// Validar parámetros obligatorios
// ================================

bool TryGetArg(string key, out string value)
{
    if (argsDict.TryGetValue(key, out value))
    {
        if (!string.IsNullOrWhiteSpace(value))
            return true;
    }

    Console.WriteLine($"❌ Falta el parámetro obligatorio: -{key}");
    return false;
}

var missing = new List<string>();
if (!TryGetArg("dns", out var dataSource)) missing.Add("-dns");
if (!TryGetArg("url", out var urls)) missing.Add("-url");
if (!TryGetArg("user1", out var user1)) missing.Add("-user1");
if (!TryGetArg("pass1", out var pass1)) missing.Add("-pass1");
if (!TryGetArg("user2", out var user2)) missing.Add("-user2");
if (!TryGetArg("pass2", out var pass2)) missing.Add("-pass2");

if (missing.Count > 0)
{
    Console.WriteLine("❌ Uso:");
    Console.WriteLine("dotnet run \\");
    Console.WriteLine("  -dns \"10.79.6.247:1521/SIGEPROD.oncop.gob.ve\" \\");
    Console.WriteLine("  -url \"http://localhost:5000\" \\");
    Console.WriteLine("  -user1 \"Consulta\" \\");
    Console.WriteLine("  -pass1 \"pumyra1584\" \\");
    Console.WriteLine("  -user2 \"AdminEgresos\" \\");
    Console.WriteLine("  -pass2 \"admin123\"");
    Environment.Exit(1);
}

// ================================
// Validar URLs
// ================================

var urlArray = urls.Split(';', StringSplitOptions.RemoveEmptyEntries);
foreach (var url in urlArray)
{
    if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uriResult) ||
        (uriResult.Scheme != "http" && uriResult.Scheme != "https"))
    {
        Console.WriteLine($"❌ URL inválida: {url}");
        Environment.Exit(1);
    }
}

// ================================
// Construir cadenas de conexión Oracle
// ================================

string connectionString1 = $"User Id={user1};Password={pass1};Data Source={dataSource};";
string connectionString2 = $"User Id={user2};Password={pass2};Data Source={dataSource};";

// ================================
// Crear builder y inyectar configuración
// ================================

var builder = WebApplication.CreateBuilder(args);

// Establecer URLs
builder.WebHost.UseUrls(urls.Split(';', StringSplitOptions.RemoveEmptyEntries));

// Inyectar cadenas de conexión en IConfiguration
builder.Configuration.AddInMemoryCollection(new[]
{
    new KeyValuePair<string, string>("Oracle:User1:ConnectionString", connectionString1),
    new KeyValuePair<string, string>("Oracle:User2:ConnectionString", connectionString2),
    new KeyValuePair<string, string>("Oracle:DataSource", dataSource),
    new KeyValuePair<string, string>("Oracle:User1", user1),
    new KeyValuePair<string, string>("Oracle:User2", user2)
});



// 👇 Añade el DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db") // Cambia si usas otro motor
);

builder.Services.AddControllers();/*.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });*/

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
 
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiResponseService>();
builder.Services.AddScoped<Seeder>();
builder.Services.AddScoped<DireccionRepository>();
builder.Services.AddScoped<CargoRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<DireccionCargoRolService>();
builder.Services.AddScoped<OracleDb>();
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔽 Habilita CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // ← Acepta solicitudes de cualquier dominio
              .AllowAnyHeader()    // ← Acepta cualquier cabecera
              .AllowAnyMethod();   // ← Acepta cualquier método (GET, POST, PUT, DELETE, etc.)
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
//app.UseGlobalExceptionHandler();
app.UseAuthentication(); // ← Antes de UseAuthorization
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 🔥 Muestra errores detallados
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error"); // producción
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();