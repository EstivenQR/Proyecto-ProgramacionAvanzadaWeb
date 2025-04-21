using Examen1_LeonardoMadrigal.Models;

namespace Examen1_LeonardoMadrigal.ViewModels
{
    public class AdminViewModel
    {
        public List<Categoria> categorias { get; set; }
        public List<Devolucion> Devoluciones { get; set; }
        public List<Estado> Estados { get; set; }
        public List<Libro> Libros { get; set; } 
        public List<Multa> Multas { get; set; }
        public List<Notificaciones> Notificaciones { get; set; }
        public List<Pedido> Pedidos { get; set; }
        public List<Prestamo> Prestamos { get; set; }
        public List<Rol> Roles { get; set; }
        public List<Usuario> Usuarios { get; set; }

    }
}
