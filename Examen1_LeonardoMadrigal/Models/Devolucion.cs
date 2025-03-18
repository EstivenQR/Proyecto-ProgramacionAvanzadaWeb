namespace Examen1_LeonardoMadrigal.Models
{
    public class Devolucion
    {
        public int Id { get; set; }
        public DateTime FechaCaducidad { get; set; }
        // Estado de si en buen estado para usar o no
        public bool EstadoLibro { get; set; }

        // Relaciones con otras tablas
        // 1.
        public int PedidoId { get; set; } // Referencia a la tabla de Pedido

        // Referencia a la tabla de Pedido
        public Pedido? Pedido { get; set; }

        // 2.
        public int EstadoId { get; set; } // Referencia a la tabla de Estado

        // Referencia a la tabla de Estado
        public Estado? Estado { get; set; }
    }
}
