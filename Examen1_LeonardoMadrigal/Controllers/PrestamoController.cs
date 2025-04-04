using Examen1_LeonardoMadrigal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: Prestamo/Index
        public IActionResult Index()
        {
            var prestamos = _context.Prestamo.Include(p => p.Libro).Include(p => p.Usuario).ToList();
            return View(prestamos);
        }

        // GET: Prestamo/Details/5
        public IActionResult Details(int id)
        {
            var prestamo = _context.Prestamo.Include(p => p.Libro).Include(p => p.Usuario).FirstOrDefault(p => p.Id == id);
            if (prestamo == null)
                return NotFound();

            return View(prestamo);
        }

        // GET: Prestamo/Create
        public IActionResult Create(int? libroId)
        {
            ViewBag.LibroId = new SelectList(_context.Libro, "Id", "Titulo", libroId);
            return View();
        }

        // POST: Prestamo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                // Obtener el libro asociado
                var libro = _context.Libro.FirstOrDefault(l => l.Id == prestamo.LibroId);
                if (libro == null)
                {
                    ModelState.AddModelError("", "El libro seleccionado no existe.");
                    return View(prestamo);
                }

                // Validar stock
                if (libro.Stock <= 0)
                {
                    ModelState.AddModelError("", "No hay stock disponible para este libro.");
                    ViewBag.LibroId = new SelectList(_context.Libro, "Id", "Titulo", prestamo.LibroId);
                    return View(prestamo);
                }

                // Reducir el stock
                libro.Stock -= 1;

                // Guardar el préstamo
                _context.Prestamo.Add(prestamo);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.LibroId = new SelectList(_context.Libro, "Id", "Titulo", prestamo.LibroId);
            return View(prestamo);
        }


        // GET: Prestamo/Edit/5
        public IActionResult Edit(int id)
        {
            var prestamo = _context.Prestamo.Find(id);
            if (prestamo == null) return NotFound();

            ViewBag.Libros = _context.Libro.ToList();
            ViewBag.Usuarios = _context.Usuario.ToList();
            return View(prestamo);
        }

        // POST: Prestamo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(prestamo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Libros = _context.Libro.ToList();
            ViewBag.Usuarios = _context.Usuario.ToList();
            return View(prestamo);
        }

        // GET: Prestamo/Delete/5
        public IActionResult Delete(int id)
        {
            var prestamo = _context.Prestamo.Include(p => p.Libro).Include(p => p.Usuario).FirstOrDefault(p => p.Id == id);
            if (prestamo == null) return NotFound();

            return View(prestamo);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var prestamo = _context.Prestamo.Find(id);
            if (prestamo == null) return NotFound();

            _context.Prestamo.Remove(prestamo);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: Prestamo/ExtenderPrestamo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExtenderPrestamo(int id)
        {
            var prestamo = _context.Prestamo.Include(p => p.Libro).ThenInclude(l => l.Estado).FirstOrDefault(p => p.Id == id);
            if (prestamo == null) return NotFound();

            if (prestamo.Libro.Estado?.Nombre == "Reservado")
            {
                return BadRequest("No se puede extender el préstamo porque el libro está reservado por otro usuario.");
            }

            prestamo.FechaFin = prestamo.FechaFin.AddDays(7);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = prestamo.Id });
        }
    }
}
