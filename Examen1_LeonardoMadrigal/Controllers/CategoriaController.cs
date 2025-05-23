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
    public class CategoriaController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public CategoriaController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        // GET: Categoria
        public async Task<IActionResult> Index()
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			return View(await _context.Categoria.ToListAsync());
        }

        // GET: Categoria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Categoria categoria)
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Categoria categoria)
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			var cantidadNotificaciones = _context.Notificaciones
.Count(); // Cuenta todas las notificaciones

			ViewBag.CantidadNotificaciones = cantidadNotificaciones;
			var categoria = await _context.Categoria.FindAsync(id);
            if (categoria != null)
            {
                _context.Categoria.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }
    }
}
