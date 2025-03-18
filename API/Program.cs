using Microsoft.EntityFrameworkCore;
using Examen1_LeonardoMadrigal.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProyectoLibreriaContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("Examen1")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//app.MapGet("/api/routes", async (ProyectoLibreriaContext context) =>
//{
//    var rutas = await context.Rutas.ToListAsync();
//    if (rutas.Count > 0)
//        return Results.Ok(rutas);
//    return Results.NoContent();

//});

//app.MapGet("/api/routes/{id}", async (int id, ProyectoLibreriaContext context) =>
//{
//    var ruta = await context.Rutas.FindAsync(id);
//    if (ruta != null)
//        return Results.Ok(ruta);
//    return Results.NotFound();
//});


app.Run();

