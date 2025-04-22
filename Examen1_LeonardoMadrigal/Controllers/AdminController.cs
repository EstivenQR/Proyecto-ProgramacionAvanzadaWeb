using Examen1_LeonardoMadrigal.Models;
using Examen1_LeonardoMadrigal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Examen1_LeonardoMadrigal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public AdminController(ProyectoLibreriaContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
        {
            // Cantidad de libros para el dashboard
            var totalLibros = await _context.Libro.CountAsync();

            var totalUsuariosActivos = await _context.Usuario.CountAsync(u => u.EstadoId == 1);
            var totalUsuariosInactivos = await _context.Usuario.CountAsync(u => u.EstadoId != 2);


            var topLibros = await _context.Libro
                .Select(e => new TopEventoViewModel
                {
                    Titulo = e.Titulo,
                    // Cuenta la cantidad de stock de libros
                    TotalStock = e.Stock,
                })
                .OrderByDescending(e => e.TotalStock)
                .Take(5)
                .ToListAsync();


            // Vistas de CRUD

            var categoriasContext = _context.Categoria;
            var librosContext = _context.Libro;
            var estadosContext = _context.Estado;
            var multasContext = _context.Multa;
            var notificacionesContext = _context.Notificacion;
            var pedidosContext = _context.Pedido;
            var rolesContext = _context.Rol;
            var usuariosContext = _context.Usuario;
            var prestamosContext = _context.Prestamo;
            var devolucionesContext = _context.Devolucion;

            // Vista Modelo del Dashboard
            var dashboardViewModel = new DashboardViewModel
            {
                TotalLibros = totalLibros,
                TotalUsuariosActivos = totalUsuariosActivos,
                TotalUsuariosInactivos = totalUsuariosInactivos,
                TopEventos = topLibros
            };


            // Modelo del CRUD 
            var ViewModel = new AdminViewModel
            {
                categorias = await categoriasContext.ToListAsync(),
                Libros = await librosContext.ToListAsync(),
                Estados = await estadosContext.ToListAsync(),
                Multas = await multasContext.ToListAsync(),
                Notificaciones = await notificacionesContext.ToListAsync(),
                Pedidos = await pedidosContext.ToListAsync(),
                Roles = await rolesContext.ToListAsync(),
                Usuarios = await usuariosContext.ToListAsync(),
                Prestamos = await prestamosContext.ToListAsync(),
                Devoluciones = await devolucionesContext.ToListAsync()
            };
    
            return View(ViewModel);
        }

        public async Task<IActionResult> Dashboard()
        {
            // Cantidad de libros para el dashboard
            var totalLibros = await _context.Libro.CountAsync();

            var totalUsuariosActivos = await _context.Usuario.CountAsync(u => u.EstadoId == 1);
            var totalUsuariosInactivos = await _context.Usuario.CountAsync(u => u.EstadoId == 2);


            var topLibros = await _context.Libro
                .Select(e => new TopEventoViewModel
                {
                    Titulo = e.Titulo,
                    // Cuenta la cantidad de stock de libros
                    TotalStock = e.Stock,
                })
                .OrderByDescending(e => e.TotalStock)
                .Take(5)
                .ToListAsync();

            // Vista Modelo del Dashboard
            var dashboardViewModel = new DashboardViewModel
            {
                TotalLibros = totalLibros,
                TotalUsuariosActivos = totalUsuariosActivos,
                TotalUsuariosInactivos = totalUsuariosInactivos,
                TopEventos = topLibros
            };



            return View(dashboardViewModel);
        }
    }
}
