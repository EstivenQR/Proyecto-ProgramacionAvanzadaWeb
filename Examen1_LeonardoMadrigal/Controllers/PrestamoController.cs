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
            // Obtener el ID del usuario actual desde la sesión
            var userId = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(userId))
            {
                // Si no hay usuario en la sesión, redirigir a la página de login
                return RedirectToAction("Login", "Usuario");
            }

            var proyectoLibreriaContext = _context.Prestamo
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .Where(d => d.Usuario.Username == userId);

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
            // 1. Obtener el usuario actual desde la sesión
            var userId = HttpContext.Session.GetString("usuario");  // Obtener el nombre de usuario desde la sesión
            if (string.IsNullOrEmpty(userId))  // Verificar si el usuario está autenticado
            {
                return RedirectToAction("Login", "Usuario");  // Redirigir al login si no está autenticado
            }

            // 2. Obtener el usuario de la base de datos
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Username == userId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // 3. Obtener el id del libro a reservar
            var libro = await _context.Libro.FirstOrDefaultAsync(p => p.Id == libroId);

            // 3.1 Si no se encuentra el libro, redirigir a una página de error o notificación
            if (libro == null)
            {
                ModelState.AddModelError("", "Libro no encontrado.");
                return View();
            }

            // 3.2 Se actualiza el estado del libro a "Inactivo" solo si el stock es 0
            if (libro.Stock == 0)
            {
                libro.EstadoId = 2;
            }

            // 3.3 Verificar si hay stock disponible
            if (libro.Stock <= 0)
            {
                ModelState.AddModelError("", "No hay stock disponible para este libro.");
                return View();
            }

            // 4. Reducir el stock del libro
            libro.Stock -= 1;

            // 5. Crear el nuevo préstamo
            var prestamo = new Prestamo
            {
                // Asignar el ID del libro al préstamo
                LibroId = libro.Id,
                // Asignar el ID del usuario al préstamo
                UsuarioId = usuario.Id,
                // Establecer la fecha de inicio como la fecha actual
                FechaInicio = DateTime.Now,
                // Establecer la fecha de fin con 7 días de duración
                FechaFin = DateTime.Now.AddDays(7),
                // Marcar el préstamo como reservado
                EstaReservado = true
            };

            // 6. Agregar el préstamo al contexto y guardar los cambios
            _context.Add(prestamo);
            await _context.SaveChangesAsync();

            // 7. Redirigir a la acción de IndexUsuarioActual para mostrar los préstamos
            //return RedirectToAction(nameof(IndexUsuarioActual));
            return RedirectToAction("Index", "Home");
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
            // Se obtiene el usuario actual
            //var userId = HttpContext.Session.GetString("usuario");  // Obtener el nombre de usuario desde la sesión
            //if (string.IsNullOrEmpty(userId))  // Verificar si el usuario está autenticado
            //{
            //    return RedirectToAction("Login", "Usuario");  // Redirigir al login si no está autenticado
            //}
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

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {state.Key} - Error: {error.ErrorMessage}");
                    }
                }
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
			var prestamo = await _context.Prestamo
				.Include(p => p.Libro)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (prestamo != null)
			{
				// Aumentar stock del libro si existe
				if (prestamo.Libro != null)
				{
					prestamo.Libro.Stock += 1;
				}

				_context.Prestamo.Remove(prestamo);
				await _context.SaveChangesAsync();
			}

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

            return RedirectToAction("IndexUsuarioActual", "Devolucion");
            //return RedirectToAction(nameof(IndexUsuarioActual), new { id = prestamo.Id });
        }


    }
}
