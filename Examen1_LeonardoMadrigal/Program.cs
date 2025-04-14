using Examen1_LeonardoMadrigal.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar DbContext
builder.Services.AddDbContext<ProyectoLibreriaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProyectoLibreria"),
    b => b.MigrationsAssembly("Examen1_LeonardoMadrigal"))
);

// Habilitar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true; // Seguridad para evitar acceso desde JavaScript
    options.Cookie.IsEssential = true; // Asegurar que la cookie de sesión siempre esté habilitada
});

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Usuario/Login"; // Ruta de inicio de sesión
        options.AccessDeniedPath = "/Home/AccessDenied/"; // Ruta de acceso denegado
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("2")); // Asegurar que solo los admin puedan entrar
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); 

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
