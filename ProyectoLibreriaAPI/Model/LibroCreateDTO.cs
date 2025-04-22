namespace ProyectoLibreriaAPI.Model
{
	public class LibroCreateDTO
	{
		public string Titulo { get; set; }
		public int Stock { get; set; }
		public string Autor { get; set; }
		public DateTime FechaLanzamiento { get; set; }
		public string Editorial { get; set; }
		public string Sinopsis { get; set; }
		public decimal Precio { get; set; }
		public string ImagenPortada { get; set; }
		public int CategoriaId { get; set; }
		public int EstadoId { get; set; }
	}
}
