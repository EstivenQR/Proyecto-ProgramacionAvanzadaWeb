namespace Examen1_LeonardoMadrigal.Models
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relaciones con las tablas
        public IEnumerable<Usuario>? Usuarios { get; set; }
        public IEnumerable<Libro>? Libros { get; set; }

        // Referencia a que hay una llave foranea en la tabla de Devolucion
        public IEnumerable<Devolucion>? Devoluciones { get; set; }

        // Referencia a que hay una llave foranea en la tabla de Multa
        public IEnumerable<Multa>? Multas { get; set; }

        // Referencia a que hay una llave foranea en la tabla de pedido
        public IEnumerable<Pedido>? Pedidos { get; set; }
    }
}
