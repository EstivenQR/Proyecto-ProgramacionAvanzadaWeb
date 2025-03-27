namespace Examen1_LeonardoMadrigal.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relaciones con las tablas
        public IEnumerable<Libro>? Libros { get; set; }
    }
}
