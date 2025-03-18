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

        // Relaciones con las tablas

        // 1.
        public int CategoriaId { get; set; } // Referencia a la tabla de Categoria

        // Referencia a la tabla de categoria
        public Categoria? Categoria { get; set; }

        // 2. 
        public int EstadoId { get; set; } // Referencia a la tabla de Estado

        // Referencia a la tabla de estado
        public Estado? Estado { get; set; }

        // 3. 
        public int NotificacionId { get; set; } // Referencia a la tabla de Notificacion

        // Referencia a la tabla de notificacion
        public Notificaciones? Notificacion { get; set; }

        // Referencia a que hay una llave foranea en la tabla de pedido
        public IEnumerable<Pedido>? Pedidos { get; set; }
    }
}
