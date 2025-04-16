using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen1_LeonardoMadrigal.Models;
using Examen1_LeonardoMadrigal.ViewModels;

namespace Examen1_LeonardoMadrigal.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public PrestamoController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        // GET: Prestamo
        // MUESTRA TODOS LOS PRESTAMOS PARA EL ADMIN
        public async Task<IActionResult> Index()
        {
            var proyectoLibreriaContext = _context.Prestamo.Include(p => p.Libro).Include(p => p.Usuario);
            return View(await proyectoLibreriaContext.ToListAsync());
        }

        // MUESTRA LOS PRESTAMOS SOLO DEL USUARIO ACTUAL
        public async Task<IActionResult> IndexUsuarioActual()
        {
            // Obtener el ID del usuario actual
            var userId = User.Identity.Name;
            var proyectoLibreriaContext = _context.Prestamo.Include(p => p.Libro).Include(p => p.Usuario).Where(d => d.Usuario.Username == userId);
            return View(await proyectoLibreriaContext.ToListAsync());

        }

        // GET: Prestamo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamo/Create
        public async Task<IActionResult> Create(int? libroId)
        {
            // 1. Obtener el usuario actual
            var userId = User.Identity.Name;
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Username == userId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }


            // 2. Obtener el id del libro a reservar
            var libro = await _context.Libro.Include(p => p.Prestamos).FirstOrDefaultAsync(p => p.Id == libroId);

                // 2.1 Se actualizar el estado del libro a "Inactivo" SOLO SI el stock es 0 
                if (libro.Stock == 0)
                {
                    libro.EstadoId = 2;
                }

                // 2.2 Actualizar el stock del libro con un libro menos
                if (libro.Stock <= 0)
                {
                    ModelState.AddModelError("", "No hay stock disponible para este libro.");
                    //ViewBag.LibroId = new SelectList(_context.Libro, "Id", "Titulo", prestamo.LibroId);
                    //return View(prestamo);
                }
            // Se le quita un libro al stock
            libro.Stock -= 1;


            var prestamo = new Prestamo
            {
                // Se le asigna el id del libro al prestamo
                LibroId = libro.Id,
                // Se le asigna el id del usuario al prestamo
                UsuarioId = usuario.Id,
                // Se coloca la fecha de inicio como la fecha actual
                FechaInicio = DateTime.Now,
                // Se le da al usuario solo 7 dias de reserva al libro
                FechaFin = DateTime.Now.AddDays(7),
                // Poner el atributo EstaReservado a true
                EstaReservado = true
            };

            _context.Add(prestamo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IndexUsuarioActual));

        }

        // GET: Prestamo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Autor", prestamo.LibroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", prestamo.UsuarioId);
            return View(prestamo);
        }

        // POST: Prestamo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LibroId,UsuarioId,FechaInicio,FechaFin,EstaReservado")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Autor", prestamo.LibroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Apellido", prestamo.UsuarioId);
            return View(prestamo);
        }

        // GET: Prestamo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamo.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamo.Remove(prestamo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamo.Any(e => e.Id == id);
        }

        // POST: Prestamo/ExtenderPrestamo/5
        public IActionResult ExtenderPrestamo(int prestamoId)
        {
            // Se obtiene el préstamo por su ID
            var prestamo = _context.Prestamo.Include(p => p.Libro).ThenInclude(l => l.Estado).FirstOrDefault(p => p.Id == prestamoId);
            if (prestamo == null) return NotFound();

            // Se aumentan 7 mas días al préstamo 
            prestamo.FechaFin = prestamo.FechaFin.AddDays(7);
            _context.SaveChanges();

            // Se redirecciona a la vista de detalles del préstamo
            return RedirectToAction(nameof(Details), new { id = prestamo.Id });
        }


    }
}
