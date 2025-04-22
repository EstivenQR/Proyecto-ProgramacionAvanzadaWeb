using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Examen1_LeonardoMadrigal.Models;
using Microsoft.EntityFrameworkCore;
using ProyectoLibreriaAPI.Model;


namespace PAWMartesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        // Contexto de la base de datos
        private readonly ProyectoLibreriaContext _context;
        // Constructor
        public LibroController(ProyectoLibreriaContext context)
        {
            _context = context;
        }

		// GET: /api/libros - Lista de libros
		[HttpGet]
		public async Task<ActionResult<IEnumerable<object>>> GetAllLibros()
		{
			var libros = await _context.Libro
				.Select(l => new {
					l.Titulo,
					l.Autor,
					l.FechaLanzamiento
				})
				.ToListAsync();

			return Ok(libros);
		}

		// GET: /api/libros/{id} - Obtener libro por ID
		[HttpGet("{id}")]
		public async Task<IActionResult> GetLibroById(int id)
		{
			var libro = await _context.Libro
				.Where(l => l.Id == id)
				.Select(l => new {
					l.Titulo,
					l.Autor,
					l.FechaLanzamiento
				})
				.FirstOrDefaultAsync();

			if (libro == null)
			{
				return NotFound();
			}

			return Ok(libro);
		}

		[HttpPost]
		public async Task<IActionResult> CrearLibro([FromBody] LibroCreateDTO dto)
		{
			var libro = new Libro
			{
				Titulo = dto.Titulo,
				Stock = dto.Stock,
				Autor = dto.Autor,
				FechaLanzamiento = dto.FechaLanzamiento,
				Editorial = dto.Editorial,
				Sinopsis = dto.Sinopsis,
				Precio = dto.Precio,
				CategoriaId = dto.CategoriaId,
				EstadoId = dto.EstadoId
			};
			_context.Libro.Add(libro);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetLibroById), new { id = libro.Id }, libro);

		}


	}
}
