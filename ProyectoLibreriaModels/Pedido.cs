namespace Examen1_LeonardoMadrigal.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }

        // Relaciones a otras tablas
        // 1.
        public int LibroId { get; set; } // Referencia a la tabla de Libro

        // Referencia a la tabla de libro
        public Libro? Libro { get; set; }

        // 2.
        public int UsuarioId { get; set; } // Referencia a la tabla de Usuario

        // Referencia a la tabla de Usuario
        public Usuario? Usuario { get; set; }

        // 3.
        public int EstadoId { get; set; } // Referencia a la tabla de Estado

        // Referencia a la tabla de Estado
        public Estado? Estado { get; set; }

    }
}
