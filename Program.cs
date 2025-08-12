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

var builder = WebApplication.CreateBuilder(args);


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


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
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