using Microsoft.EntityFrameworkCore;

namespace Examen1_LeonardoMadrigal.Models
{
    public class ProyectoLibreriaContext : DbContext
    {
        public ProyectoLibreriaContext(DbContextOptions<ProyectoLibreriaContext> options) : base(options)
        {
        }

        // Tablas o las entidades de la base de datos
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Devolucion> Devolucion { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Libro> Libro { get; set; }
        public DbSet<Multa> Multa { get; set; }
        public DbSet<Notificaciones> Notificacion { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Prestamo> Prestamo { get; set; }

        // Sobrescribir el evento para modificar la creación de la instancia y sus propiedades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Se configura la tabla de Categoria
            modelBuilder.Entity<Categoria>(Categoria =>
            {
                Categoria.HasKey(e => e.Id);
                Categoria.Property(n => n.Nombre).HasMaxLength(50).IsRequired();
            });

            // Se configura la tabla de Devolucion
            modelBuilder.Entity<Devolucion>(Devolucion =>
            {
                Devolucion.HasKey(e => e.Id);
            });

            // Se configura la tabla de Estado     
            modelBuilder.Entity<Estado>(Estado =>
            {
                Estado.HasKey(e => e.Id);
            });

            // Se configura la tabla de Libro
            modelBuilder.Entity<Libro>(Libro =>
            {
                Libro.HasKey(e => e.Id);
                Libro.Property(n => n.Titulo).HasMaxLength(110).IsRequired();
                Libro.Property(n => n.Stock).IsRequired();
                Libro.Property(n => n.Autor).HasMaxLength(100).IsRequired();
                Libro.Property(n => n.FechaLanzamiento).IsRequired();
                Libro.Property(n => n.Editorial).HasMaxLength(100).IsRequired();
                Libro.Property(n => n.Sinopsis).HasMaxLength(500);
            });

            // Se configura la tabla de Multa
            modelBuilder.Entity<Multa>(Multa =>
            {
                Multa.HasKey(e => e.Id);
                Multa.Property(n => n.Descripcion).HasMaxLength(200).IsRequired();
                Multa.Property(c => c.PrecioMulta).IsRequired();
            });

            // Se configura la tabla de Notificaciones
            modelBuilder.Entity<Notificaciones>(Notificacion =>
            {
                Notificacion.HasKey(e => e.Id);
                Notificacion.Property(n => n.Mensaje).HasMaxLength(150).IsRequired();
                Notificacion.Property(c => c.FechaSolicitud).IsRequired();
            });

            // Se configura la tabla de Pedido
            modelBuilder.Entity<Pedido>(Pedido =>
            {
                Pedido.HasKey(e => e.Id);
                Pedido.Property(n => n.FechaPedido).IsRequired();
            });

            // Se configura la tabla de Rol
            modelBuilder.Entity<Rol>(Rol =>
            {
                Rol.HasKey(e => e.Id);
                Rol.Property(n => n.Nombre).HasMaxLength(30).IsRequired();
            });

            // Se configura la tabla de Usuario
            modelBuilder.Entity<Usuario>(Usuario =>
            {
                Usuario.HasKey(e => e.Id);
                Usuario.Property(n => n.Nombre).HasMaxLength(70).IsRequired();
                Usuario.Property(n => n.Apellido).HasMaxLength(70).IsRequired();
                Usuario.Property(c => c.Correo).IsRequired().HasMaxLength(300).IsUnicode(true);
                Usuario.Property(c => c.Telefono).IsRequired().HasMaxLength(30);
                Usuario.Property(c => c.Username).IsRequired().HasMaxLength(120);
                Usuario.Property(c => c.Password).HasMaxLength(150);
            });

            // Relacion de Usuario con Rol y Estado
            modelBuilder.Entity<Usuario>().HasOne(u => u.Rol).WithMany(r => r.Usuarios).HasForeignKey(u => u.RolId);
            modelBuilder.Entity<Usuario>().HasOne(u => u.Estado).WithMany(e => e.Usuarios).HasForeignKey(u => u.EstadoId);

            // Configuración de la tabla Libro
            modelBuilder.Entity<Libro>().HasOne(l => l.Categoria).WithMany(c => c.Libros).HasForeignKey(l => l.CategoriaId);
            modelBuilder.Entity<Libro>().HasOne(l => l.Estado).WithMany(e => e.Libros).HasForeignKey(l => l.EstadoId);
            modelBuilder.Entity<Libro>().HasOne(l => l.Notificacion).WithMany(n => n.Libros).HasForeignKey(l => l.NotificacionId);

            // Relación de Pedido
            modelBuilder.Entity<Pedido>().HasOne(p => p.Libro).WithMany(l => l.Pedidos).HasForeignKey(p => p.LibroId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Pedido>().HasOne(p => p.Usuario).WithMany(u => u.Pedidos).HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Pedido>().HasOne(p => p.Estado).WithMany(e => e.Pedidos).HasForeignKey(p => p.EstadoId);

            // Configuración de la tabla Devolucion
            modelBuilder.Entity<Devolucion>().HasOne(d => d.Pedido).WithMany(p => p.Devoluciones).HasForeignKey(d => d.PedidoId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Devolucion>().HasOne(d => d.Estado).WithMany(e => e.Devoluciones).HasForeignKey(d => d.EstadoId);

            // Configuración de la tabla Multa
            modelBuilder.Entity<Multa>().HasOne(m => m.Usuario).WithMany(u => u.Multas).HasForeignKey(m => m.UsuarioId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Multa>().HasOne(m => m.Estado).WithMany(e => e.Multas).HasForeignKey(m => m.EstadoId);

            // Configuración de la tabla Notificaciones
            modelBuilder.Entity<Notificaciones>().HasKey(n => n.Id);

            // Agrega la configuración de Prestamo
            modelBuilder.Entity<Prestamo>(Prestamo =>
            {
                Prestamo.HasKey(e => e.Id);
                Prestamo.Property(p => p.FechaInicio).IsRequired();
                Prestamo.Property(p => p.FechaFin).IsRequired();
                Prestamo.HasOne(p => p.Libro).WithMany(l => l.Prestamos).HasForeignKey(p => p.LibroId);
             });
        }
    }
}
