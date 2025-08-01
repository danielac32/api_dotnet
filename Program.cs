// Program.cs
using backend_ont_2.data; // Ajusta según tu namespace
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// 👇 Añade el DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db") // Cambia si usas otro motor
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();