using Examen1_LeonardoMadrigal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Examen1_LeonardoMadrigal.Controllers
{
    public class NotificacionController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public NotificacionController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        // GET: Notificacion/Index
        public async Task<IActionResult> Index()
        {
            var notificaciones = await _context.Notificacion.ToListAsync();
            return View(notificaciones); // Mostrar todas las notificaciones
        }

        // GET: Notificacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificacion = await _context.Notificacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificacion == null)
            {
                return NotFound();
            }

            return View(notificacion); // Mostrar los detalles de la notificación
        }

        // GET: Notificacion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notificacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mensaje,FechaSolicitud")] Notificaciones notificacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirige a la lista de notificaciones
            }
            return View(notificacion);
        }

        // GET: Notificacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificacion = await _context.Notificacion.FindAsync(id);
            if (notificacion == null)
            {
                return NotFound();
            }
            return View(notificacion);
        }

        // POST: Notificacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mensaje,FechaSolicitud")] Notificaciones notificacion)
        {
            if (id != notificacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificacionExists(notificacion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirige a la lista de notificaciones
            }
            return View(notificacion);
        }

        // GET: Notificacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificacion = await _context.Notificacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificacion == null)
            {
                return NotFound();
            }

            return View(notificacion); // Mostrar confirmación para eliminar la notificación
        }

        // POST: Notificacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notificacion = await _context.Notificacion.FindAsync(id);
            if (notificacion != null)
            {
                _context.Notificacion.Remove(notificacion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirige a la lista de notificaciones
        }

        private bool NotificacionExists(int id)
        {
            return _context.Notificacion.Any(e => e.Id == id);
        }
    }
}