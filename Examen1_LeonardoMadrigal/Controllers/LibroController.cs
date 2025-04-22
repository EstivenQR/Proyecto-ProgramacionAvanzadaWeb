using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen1_LeonardoMadrigal.Models;

namespace Examen1_LeonardoMadrigal.Controllers
{
    public class LibroController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public LibroController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> UsuarioIndex(string searchString)
        {
            // Se obtiene la lista de libros con sus relaciones
            var libros = _context.Libro
                .Include(l => l.Categoria)
                .Include(l => l.Estado)
                .Include(l => l.Notificacion)
                .AsQueryable();  // Permite aplicar filtros din�micos

            // Si el par�metro searchString no es nulo ni vac�o, aplica el filtro
            if (!string.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(l => l.Titulo.Contains(searchString)
                                           || l.Autor.Contains(searchString)
                                           || l.Editorial.Contains(searchString));
            }

            // Devuelve la vista con los libros filtrados
            return View(await libros.ToListAsync());
        }



        // GET: Libro
        public async Task<IActionResult> Index(string searchString)
        {
            var libros = _context.Libro
                .Include(l => l.Categoria)
                .Include(l => l.Estado)
                .Include(l => l.Notificacion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(l => l.Titulo.Contains(searchString) || l.Autor.Contains(searchString));
            }

            return View(await libros.ToListAsync());
        }

        // GET: Libro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libro
                .Include(l => l.Categoria)
                .Include(l => l.Estado)
                .Include(l => l.Notificacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libro/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre");
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id");
            ViewData["NotificacionId"] = new SelectList(_context.Notificacion, "Id", "Mensaje");
            return View();
        }

        // POST: Libro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Stock,Autor,FechaLanzamiento,Editorial,Sinopsis,CategoriaId,EstadoId,NotificacionId,Precio")] Libro libro, IFormFile imagenPortada)
        {
            // Validaci�n
            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", libro.CategoriaId);
                ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", libro.EstadoId);
                ViewData["NotificacionId"] = new SelectList(_context.Notificacion, "Id", "Mensaje", libro.NotificacionId);
                return View(libro);
            }

            // Procesar la imagen si se sube un archivo
            if (imagenPortada != null && imagenPortada.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imagenPortada.CopyToAsync(ms);
                    libro.ImagenPortada = ms.ToArray();  // Guardar la imagen en el campo ImagenPortada
                }
            }

            _context.Add(libro);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index)); // Redirigir al �ndice de libros
            return RedirectToAction("Index", "Admin");
        }

        // GET: Libro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libro.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", libro.CategoriaId);
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", libro.EstadoId);
            ViewData["NotificacionId"] = new SelectList(_context.Notificacion, "Id", "Mensaje", libro.NotificacionId);
            return View(libro);
        }

        // POST: Libro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Stock,Autor,FechaLanzamiento,Editorial,Sinopsis,CategoriaId,EstadoId,NotificacionId,Precio")] Libro libro, IFormFile imagenPortada)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            // Verificar errores en ModelState
            if (!ModelState.IsValid)
            {
                // Mostrar errores en la consola para depuraci�n
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                    //_logger.LogError("Error en ModelState: " + error.ErrorMessage); // Para depuraci�n en logs
                }

                // Recargar SelectLists
                ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", libro.CategoriaId);
                ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", libro.EstadoId);
                ViewData["NotificacionId"] = new SelectList(_context.Notificacion, "Id", "Mensaje", libro.NotificacionId);
                return View(libro);
            }

            try
            {
                // Obtener el libro actual de la base de datos
                var libroExistente = await _context.Libro.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
                if (libroExistente == null)
                {
                    return NotFound();
                }

                // Si se sube una nueva imagen, la reemplazamos; si no, mantenemos la actual
                if (imagenPortada != null && imagenPortada.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await imagenPortada.CopyToAsync(ms);
                        libro.ImagenPortada = ms.ToArray();
                    }
                }
                else
                {
                    libro.ImagenPortada = libroExistente.ImagenPortada; // Mantener la imagen actual
                }

                _context.Update(libro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Libro.Any(l => l.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Admin");
        }


        // GET: Libro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libro
                .Include(l => l.Categoria)
                .Include(l => l.Estado)
                .Include(l => l.Notificacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libro.FindAsync(id);
            if (libro != null)
            {
                _context.Libro.Remove(libro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return _context.Libro.Any(e => e.Id == id);
        }
    }
}
