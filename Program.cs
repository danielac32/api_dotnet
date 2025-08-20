using backend_ont_2.data;
using Microsoft.EntityFrameworkCore;
using backend_ont_2.features.user.repositories;
using backend_ont_2.features.user.service;
using backend_ont_2.features.user.service.auth;
using backend_ont_2.shared.apiResponse;
using backend_ont_2.seeder;
using backend_ont_2.DCR;
using backend_ont_2.DCR.Services;
using backend_ont_2.OracleDbProject;
using backend_ont_2.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================================
// 🔐 Cargar configuración desde appsettings.json
// ================================

var oracleConfig = builder.Configuration.GetSection("Oracle");
var urls = builder.Configuration["Urls"] ?? "http://localhost:5288";

var dataSource = oracleConfig["DataSource"];
var user1 = oracleConfig["User1"];
var pass1 = oracleConfig["Pass1"];
var user2 = oracleConfig["User2"];
var pass2 = oracleConfig["Pass2"];

// ================================
// ✅ Validar que la configuración exista
// ================================

if (string.IsNullOrEmpty(dataSource) ||
    string.IsNullOrEmpty(user1) ||
    string.IsNullOrEmpty(pass1) ||
    string.IsNullOrEmpty(user2) ||
    string.IsNullOrEmpty(pass2))
{
    Console.WriteLine("❌ Configuración de Oracle incompleta en appsettings.json");
    Environment.Exit(1);
}

// Validar URLs
var urlArray = urls.Split(';', StringSplitOptions.RemoveEmptyEntries);
foreach (var url in urlArray)
{
    if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uriResult) ||
        (uriResult.Scheme != "http" && uriResult.Scheme != "https"))
    {
        Console.WriteLine($"❌ URL inválida en configuración: {url}");
        Environment.Exit(1);
    }
}

// ================================
// 🔗 Construir cadenas de conexión
// ================================

string connectionString1 = $"User Id={user1};Password={pass1};Data Source={dataSource};";
string connectionString2 = $"User Id={user2};Password={pass2};Data Source={dataSource};";

// Inyectar conexiones en IConfiguration
builder.Configuration.AddInMemoryCollection(new[]
{
    new KeyValuePair<string, string>("Oracle:User1:ConnectionString", connectionString1),
    new KeyValuePair<string, string>("Oracle:User2:ConnectionString", connectionString2),
    new KeyValuePair<string, string>("Oracle:DataSource", dataSource),
    new KeyValuePair<string, string>("Oracle:User1", user1),
    new KeyValuePair<string, string>("Oracle:User2", user2)
});

// ================================
// 🛠️ Configurar servicios
// ================================

builder.WebHost.UseUrls(urls);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db")
);

builder.Services.AddControllers();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
/*
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}*/
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();