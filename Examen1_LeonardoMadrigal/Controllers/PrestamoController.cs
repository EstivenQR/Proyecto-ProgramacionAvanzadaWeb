using Examen1_LeonardoMadrigal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Examen1_LeonardoMadrigal.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public PrestamoController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        // GET: HomeController/Index
        public IActionResult Index()
        {
            var libros = _context.Libro.Include(l => l.Categoria)
                                        .Include(l => l.Estado)
                                        .Include(l => l.Notificacion)
                                        .ToList();
            return View(libros);
        }

        // GET: HomeController/Details/5
        public IActionResult Details(int id)
        {
            var libro = _context.Libro.Include(l => l.Categoria)
                                       .Include(l => l.Estado)
                                       .Include(l => l.Notificacion)
                                       .FirstOrDefault(l => l.Id == id);

            if (libro == null)
                return NotFound();

            return View(libro);
        }

        // GET: HomeController/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = _context.Categoria.ToList();
            ViewBag.Estados = _context.Estado.ToList();
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Libro.Add(libro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = _context.Categoria.ToList();
            ViewBag.Estados = _context.Estado.ToList();
            return View(libro);
        }

        // GET: HomeController/Edit/5
        public IActionResult Edit(int id)
        {
            var libro = _context.Libro.Include(l => l.Categoria)
                                       .Include(l => l.Estado)
                                       .FirstOrDefault(l => l.Id == id);
            if (libro == null) return NotFound();

            ViewBag.Categorias = _context.Categoria.ToList();
            ViewBag.Estados = _context.Estado.ToList();
            return View(libro);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Libro libro)
        {
            if (id != libro.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(libro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = _context.Categoria.ToList();
            ViewBag.Estados = _context.Estado.ToList();
            return View(libro);
        }

        // GET: HomeController/Delete/5
        public IActionResult Delete(int id)
        {
            var libro = _context.Libro.Include(l => l.Categoria)
                                       .Include(l => l.Estado)
                                       .FirstOrDefault(l => l.Id == id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        // POST: HomeController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = _context.Libro.Find(id);
            if (libro == null) return NotFound();

            _context.Libro.Remove(libro);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: HomeController/ExtenderPrestamo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExtenderPrestamo(int id)
        {
            var prestamo = _context.Prestamo.Include(p => p.Libro)
                                             .Include(p => p.Libro.Estado)
                                             .FirstOrDefault(p => p.Id == id);

            if (prestamo == null) return NotFound();

            if (prestamo.Libro.Estado.Nombre == "Reservado")
            {
                return BadRequest("No se puede extender el préstamo porque el libro está reservado por otro usuario.");
            }

            prestamo.FechaFin = prestamo.FechaFin.AddDays(7); // Extiende el préstamo por 7 días
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = prestamo.Id });
        }

        // GET: HomeController/Prestamos
        public IActionResult Prestamos()
        {
            var prestamos = _context.Prestamo.Include(p => p.Libro)
                                              .Include(p => p.UsuarioId)
                                              .ToList();
            return View(prestamos);
        }
    }
}
