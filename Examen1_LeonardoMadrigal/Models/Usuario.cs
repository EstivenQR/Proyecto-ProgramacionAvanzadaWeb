using System.Drawing;

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

        // 1.
        public int RolId { get; set; } // Referencia a la tabla de Rol

        // Referencia a la tabla de rol
        public Rol? Rol { get; set; }

        // 2. 
        public int EstadoId { get; set; } // Referencia a la tabla de Estado

        // Referencia a la tabla de estado
        public Estado? Estado { get; set; }

        // Referencia a que hay una llave foranea en la tabla de pedido
        public IEnumerable<Pedido>? Pedidos { get; set; }

        // Referencia a que hay una llave foranea en la tabla de Multa
        public IEnumerable<Multa>? Multas { get; set; }





    }
}
