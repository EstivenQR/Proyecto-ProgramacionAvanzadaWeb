using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen1_LeonardoMadrigal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Examen1_LeonardoMadrigal.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly ProyectoLibreriaContext _context;

        public UsuarioController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var proyectoLibreriaContext = _context.Usuario.Include(u => u.Estado).Include(u => u.Rol);
            return View(await proyectoLibreriaContext.ToListAsync());
        }

        // GET: Usuario/Details/5
        //[Authorize(Roles = "2")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.Estado)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id");
            ViewData["RolId"] = new SelectList(_context.Rol, "Id", "Nombre");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Correo,Telefono,Username,Password,RolId,EstadoId")] Usuario usuario, IFormFile imagenPerfil)
        {
            ModelState.Remove("imagenPerfil"); // Remover validaci�n si no es obligatorio
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Clave: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"Error: {err.ErrorMessage}");
                    }
                }

                ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", usuario.EstadoId);
                ViewData["RolId"] = new SelectList(_context.Rol, "Id", "Nombre", usuario.RolId);
                return View(usuario);
            }

            // Procesar la imagen si se sube un archivo
            if (imagenPerfil != null && imagenPerfil.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imagenPerfil.CopyToAsync(ms);
                    usuario.RutaImagen = ms.ToArray();
                }
            }

            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", usuario.EstadoId);
            ViewData["RolId"] = new SelectList(_context.Rol, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Correo,Telefono,RutaImagen,Username,Password,RolId,EstadoId")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", usuario.EstadoId);
            ViewData["RolId"] = new SelectList(_context.Rol, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.Estado)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Usuario, string contrase�a)
        {
            Console.WriteLine($"Usuario: {Usuario}, Contrase�a: {contrase�a}");
            bool loginExitoso = await _context.LoginUsuario(Usuario, contrase�a);

            if (loginExitoso)
            {
                var usuario = await _context.ObtenerUsuario(Usuario, contrase�a);
                if (usuario != null)
                {
                    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Username),
        new Claim(ClaimTypes.Role, usuario.RolId.ToString()) // Asegurar que el rol sea un string
    };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Usuario o contrase�a incorrectos";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Usuario usuario, IFormFile imagenPerfil)
        {
            ModelState.Remove("imagenPerfil"); // Se remueve la validaci�n si no es obligatorio
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorRegistro = "Verifique los datos ingresados.";
                return View("Login"); // Se retorna a la misma vista de login
            }

            // Se asignan valores predeterminados
            usuario.RolId = 1;
            usuario.EstadoId = 1;

            // Procesar la imagen si se sube un archivo
            if (imagenPerfil != null && imagenPerfil.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imagenPerfil.CopyToAsync(ms);
                    usuario.RutaImagen = ms.ToArray();
                }
            }

            _context.Add(usuario);
            await _context.SaveChangesAsync();
            ViewBag.MensajeRegistro = "Registro exitoso, ahora puedes iniciar sesi�n.";
            return View("Login"); // Se retorna a la misma vista de login con mensaje de �xito
        }





        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
