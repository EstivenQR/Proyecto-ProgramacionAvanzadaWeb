using System.ComponentModel.DataAnnotations.Schema;

namespace Examen1_LeonardoMadrigal.Models
{
    [Table("Notificacion")]

    public class Notificaciones
    {
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaSolicitud { get; set; }


        // Relaciones con las tablas
        public int? LibroId { get; set; }        // Clave foránea
        public Libro Libro { get; set; }        // Propiedad de navegación
    }
}
