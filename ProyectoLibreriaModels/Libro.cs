namespace Examen1_LeonardoMadrigal.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Stock { get; set; }
        public string Autor { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string Editorial { get; set; }
        public string Sinopsis { get; set; }
        public decimal Precio { get; set; }
        public byte[]? ImagenPortada { get; set; }


        // Relaciones con las tablas

        // 1. Relación con la tabla Categoria
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // 2. Relación con la tabla Estado
        public int EstadoId { get; set; }
        public Estado? Estado { get; set; }

        // 3. Relación con la tabla Notificacion
        public int NotificacionId { get; set; }
        public Notificaciones? Notificacion { get; set; }

        // 4. Relación con la tabla Pedido
        public IEnumerable<Pedido>? Pedidos { get; set; }

        // 5. Relación con la tabla Prestamo
        public ICollection<Prestamo>? Prestamos { get; set; }
    }
}
