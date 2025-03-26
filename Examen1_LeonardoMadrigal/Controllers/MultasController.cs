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
    public class MultasController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public MultasController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        // GET: Multas
        public async Task<IActionResult> Index()
        {
            var proyectoLibreriaContext = _context.Multa.Include(m => m.Estado).Include(m => m.Usuario);
            return View(await proyectoLibreriaContext.ToListAsync());
        }

        // GET: Multas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multa = await _context.Multa
                .Include(m => m.Estado)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multa == null)
            {
                return NotFound();
            }

            return View(multa);
        }

        // GET: Multas/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Username");
            return View();
        }

        // POST: Multas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,PrecioMulta,UsuarioId,EstadoId")] Multa multa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(multa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre", multa.EstadoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Username", multa.UsuarioId);
            return View(multa);
        }

        // GET: Multas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multa = await _context.Multa.FindAsync(id);
            if (multa == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre", multa.EstadoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Username", multa.UsuarioId);
            return View(multa);
        }

        // POST: Multas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,PrecioMulta,UsuarioId,EstadoId")] Multa multa)
        {
            if (id != multa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(multa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultaExists(multa.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Nombre", multa.EstadoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Username", multa.UsuarioId);
            return View(multa);
        }

        // GET: Multas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multa = await _context.Multa
                .Include(m => m.Estado)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multa == null)
            {
                return NotFound();
            }

            return View(multa);
        }

        // POST: Multas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var multa = await _context.Multa.FindAsync(id);
            if (multa != null)
            {
                _context.Multa.Remove(multa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MultaExists(int id)
        {
            return _context.Multa.Any(e => e.Id == id);
        }
    }
}
