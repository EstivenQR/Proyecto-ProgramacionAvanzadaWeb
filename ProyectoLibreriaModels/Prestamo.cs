using Microsoft.EntityFrameworkCore;

namespace Examen1_LeonardoMadrigal.Models
{
        public class Prestamo
        {
            public int Id { get; set; }
            public int LibroId { get; set; }
            public Libro? Libro { get; set; }
            public int? UsuarioId { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public bool EstaReservado { get; set; } = false;

        //// Relaciones hacia otras tablas
        //// 1.z
        //public int LibroId { get; set; } // Referencia a la tabla de Libro

        //// Referencia a la tabla de libro
        //public Libro? Libro { get; set; }

        //// 2.
        //public int UsuarioId { get; set; } // Referencia a la tabla de Usuario

        // Referencia a la tabla de Usuario
        public virtual Usuario Usuario { get; set; }


    }
}