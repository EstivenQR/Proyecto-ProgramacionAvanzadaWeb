using Examen1_LeonardoMadrigal.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Colocar el context que esta en la carpeta models
builder.Services.AddDbContext<ProyectoLibreriaContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("ProyectoLibreria")).LogTo(Console.WriteLine, LogLevel.Information)
      .EnableSensitiveDataLogging();

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
