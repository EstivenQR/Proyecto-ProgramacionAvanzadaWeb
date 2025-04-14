using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen1_LeonardoMadrigal.Models;
using System.Security.Claims;
using Examen1_LeonardoMadrigal.ViewModels;

namespace Examen1_LeonardoMadrigal.Controllers
{
    public class DevolucionController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public DevolucionController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        // GET: Devolucion
        public async Task<IActionResult> Index()
        {
            var proyectoLibreriaContext = _context.Devolucion.Include(d => d.Estado).Include(d => d.Prestamo).Include(d => d.Usuario);
            return View(await proyectoLibreriaContext.ToListAsync());
        }

        public async Task<IActionResult> IndexUsuarioActual()
        {
            // Obtener el ID del usuario actual
            var userId = User.Identity.Name;

            // Filtrar las devoluciones por el ID del usuario actual
            var devolucionesContext = _context.Devolucion
                .Include(d => d.Estado)
                .Include(d => d.Prestamo)
                .Include(d => d.Usuario)
                .Where(d => d.Usuario.Username == userId);

            // Filtrar las devoluciones por el ID del usuario actual
            var prestamosContext = _context.Prestamo
                .Include(d => d.Usuario)
                .Where(d => d.Usuario.Username == userId);

            var viewModel = new DevolucionPrestamoUsuario_ViewModel
            {
                Devoluciones = await devolucionesContext.ToListAsync(),
                Prestamos = await prestamosContext.ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Devolucion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion
                .Include(d => d.Estado)
                .Include(d => d.Prestamo)
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // GET: Devolucion/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre");
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "Id", "Id");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido");
            return View();
        }

        // POST: Devolucion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaCaducidad,EstadoLibro,PrestamoId,EstadoId,UsuarioId")] Devolucion devolucion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(devolucion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre", devolucion.EstadoId);
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "Id", "Id", devolucion.PrestamoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", devolucion.UsuarioId);
            return View(devolucion);
        }

        // GET: Devolucion/Create
        public IActionResult CreateUsuarioActual()
        {
            //var userId = User.Identity.Name;
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre");
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "Id", "Id");
            //ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido");
            return View();
        }

        // POST: Devolucion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsuarioActual([Bind("Id,FechaCaducidad,EstadoLibro,PrestamoId,EstadoId")] Devolucion devolucion)
        {
            // En este metodo, cuando el usuario seleccione el prestamo de libro que quiere devolver, se devolvera el libro creando una devolucion

            // 1. Se obtiene el usuario actual
            var userId = User.Identity.Name;
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Username == userId);
            if (usuario != null)
            {
                return NotFound("Usuario no encontrado");
            }
            devolucion.UsuarioId = usuario.Id; // Asignar el usuario actual
            // 2. Se obtiene el prestamo seleccionado



            // 3. Se coloca el estado de la devolucion a inactivo (es decir que SI se devolvio el libro)
            devolucion.EstadoId = 2; // Asignar el estado de la devolucion a inactivo (es decir que SI se devolvio el libro)

            if (ModelState.IsValid)
            {
                _context.Add(devolucion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre", devolucion.EstadoId);
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "Id", "Id", devolucion.PrestamoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", devolucion.UsuarioId);
            return View(devolucion);
        }

        // GET: Devolucion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion.FindAsync(id);
            if (devolucion == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre", devolucion.EstadoId);
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "Id", "Id", devolucion.PrestamoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", devolucion.UsuarioId);
            return View(devolucion);
        }

        // POST: Devolucion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCaducidad,EstadoLibro,PrestamoId,EstadoId,UsuarioId")] Devolucion devolucion)
        {
            if (id != devolucion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devolucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucionExists(devolucion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre", devolucion.EstadoId);
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "Id", "Id", devolucion.PrestamoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", devolucion.UsuarioId);
            return View(devolucion);
        }

        // GET: Devolucion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion
                .Include(d => d.Estado)
                .Include(d => d.Prestamo)
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // POST: Devolucion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devolucion = await _context.Devolucion.FindAsync(id);
            if (devolucion != null)
            {
                _context.Devolucion.Remove(devolucion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucionExists(int id)
        {
            return _context.Devolucion.Any(e => e.Id == id);
        }
    }
}
