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
    }
}
