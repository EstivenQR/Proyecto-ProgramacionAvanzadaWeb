namespace Examen1_LeonardoMadrigal.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public byte[]? RutaImagen { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Relaciones con las tablas

        // 1. Relación con la tabla Rol
        public int RolId { get; set; }
        public Rol? Rol { get; set; }

        // 2. Relación con la tabla Estado
        public int EstadoId { get; set; }
        public Estado? Estado { get; set; }

        // 3. Relación con la tabla Pedido
        public IEnumerable<Pedido>? Pedidos { get; set; }

        // 4. Relación con la tabla Multa
        public IEnumerable<Multa>? Multas { get; set; }

        // 5. Relación con la tabla Prestamo
        public ICollection<Prestamo>? Prestamos { get; set; }
    }
}
