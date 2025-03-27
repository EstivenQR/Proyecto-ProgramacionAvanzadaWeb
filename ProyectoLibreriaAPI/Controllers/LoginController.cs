using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Examen1_LeonardoMadrigal.Models;

namespace PAWMartesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // Contexto de la base de datos
        private readonly ProyectoLibreriaContext _context;
        // Constructor
        public LoginController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string Usuario, string Contraseña)
        {
            // Se llama al metodo creado que a su vez llamada al procedimiento almacenado en el sql
            var Resultado = await _context.LoginUsuario(Usuario, Contraseña);
            if(Resultado)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerUsuario(string Usu, string Pass)
        {
            try
            {
                var UsuarioDB = await _context.ObtenerUsuario(Usu, Pass);
                if(UsuarioDB == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(UsuarioDB);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
