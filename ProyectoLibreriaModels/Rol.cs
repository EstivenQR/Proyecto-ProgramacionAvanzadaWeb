namespace Examen1_LeonardoMadrigal.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relaciones con las tablas
        public IEnumerable<Usuario>? Usuarios { get; set; }

    }
}
