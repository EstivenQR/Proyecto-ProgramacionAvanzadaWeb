namespace Examen1_LeonardoMadrigal.Models
{
    public class Multa
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public double PrecioMulta { get; set; }

        // Relaciones hacia otras tablas
        // 1.
        public int UsuarioId { get; set; } // Referencia a la tabla de Usuario

        // Referencia a la tabla de Usuario
        public Usuario? Usuario { get; set; }

        // 2.
        public int EstadoId { get; set; } // Referencia a la tabla de Estado

        // Referencia a la tabla de Estado
        public Estado? Estado { get; set; }
    }
}
