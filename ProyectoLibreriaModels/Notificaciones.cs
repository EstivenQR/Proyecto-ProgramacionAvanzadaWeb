namespace Examen1_LeonardoMadrigal.Models
{
    public class Notificaciones
    {
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaSolicitud { get; set; }

        // Relaciones con las tablas
        public IEnumerable<Libro>? Libros { get; set; }
    }
}
