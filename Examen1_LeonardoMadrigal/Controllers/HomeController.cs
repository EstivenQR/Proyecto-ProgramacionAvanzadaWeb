using Examen1_LeonardoMadrigal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Examen1_LeonardoMadrigal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProyectoLibreriaContext _context;

        // Agregar el constructor para el contexto
        public HomeController(ILogger<HomeController> logger, ProyectoLibreriaContext context)
        {
            _logger = logger;
            _context = context;  // Inicializamos el contexto
        }

        // Modificar la acción Index para obtener el último libro
        public async Task<IActionResult> Index()
        {
            var cantidadNotificaciones = _context.Notificaciones
    .Count(); // Cuenta todas las notificaciones

            ViewBag.CantidadNotificaciones = cantidadNotificaciones;
            // Obtener todos los libros desde la base de datos
            var libros = await _context.Libro.ToListAsync();

            // Obtener el último libro (si existe algún libro en la base de datos)
            var ultimoLibro = libros.LastOrDefault();

            // Pasar el último libro a la vista para mostrarlo arriba y todos los libros para la sección de Featured
            ViewBag.UltimoLibro = ultimoLibro;
            return View(libros);
        }

        // Acción para ver todos los libros
        public async Task<IActionResult> AllBooks()
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			// Obtener todos los libros desde la base de datos
			var libros = await _context.Libro.Include(l => l.Categoria).ToListAsync();

            // Pasar los libros a la vista
            return View(libros);
        }

        // Acción para ver los detalles de un libro
        public async Task<IActionResult> Details(int id)
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			// Obtener el libro con el id correspondiente
			var libro = await _context.Libro.Include(l => l.Categoria)
                                             .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
            {
                return NotFound();  // Si el libro no existe, mostrar una página de error.
            }

            // Pasar el libro a la vista
            return View(libro);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReducirStock(int id)
        {
            var libro = _context.Libro.FirstOrDefault(l => l.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            if (libro.Stock > 0)
            {
                libro.Stock -= 1;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("Stock agotado");
            }
        }
    }
}
