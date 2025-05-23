﻿using Microsoft.Data.SqlClient;
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
        public DbSet<Notificaciones> Notificaciones { get; set; }

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
                Libro.Property(n => n.Precio).HasPrecision(18, 2);
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

            // Configuración de la relación entre Libro y Notificaciones
            modelBuilder.Entity<Notificaciones>()
               .HasOne(n => n.Libro)
               .WithMany(l => l.Notificaciones)
               .HasForeignKey(n => n.LibroId)
               .OnDelete(DeleteBehavior.SetNull);


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

            modelBuilder.Entity<Prestamo>(Prestamo =>
            {
                Prestamo.HasKey(e => e.Id);
                Prestamo.Property(p => p.FechaInicio).IsRequired();
                Prestamo.Property(p => p.FechaFin).IsRequired();
                Prestamo.HasOne(p => p.Libro).WithMany(l => l.Prestamos).HasForeignKey(p => p.LibroId);

            });

            // Relacion de Usuario con Rol y Estado
            modelBuilder.Entity<Usuario>().HasOne(u => u.Rol).WithMany(r => r.Usuarios).HasForeignKey(u => u.RolId);
            modelBuilder.Entity<Usuario>().HasOne(u => u.Estado).WithMany(e => e.Usuarios).HasForeignKey(u => u.EstadoId);

            // Configuración de la tabla Libro
            modelBuilder.Entity<Libro>().HasOne(l => l.Categoria).WithMany(c => c.Libros).HasForeignKey(l => l.CategoriaId);
            modelBuilder.Entity<Libro>().HasOne(l => l.Estado).WithMany(e => e.Libros).HasForeignKey(l => l.EstadoId);

            // Relación de Pedido
            modelBuilder.Entity<Pedido>().HasOne(p => p.Libro).WithMany(l => l.Pedidos).HasForeignKey(p => p.LibroId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Pedido>().HasOne(p => p.Usuario).WithMany(u => u.Pedidos).HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Pedido>().HasOne(p => p.Estado).WithMany(e => e.Pedidos).HasForeignKey(p => p.EstadoId);

            // Configuración de la tabla Devolucion
            modelBuilder.Entity<Devolucion>().HasOne(d => d.Prestamo).WithMany(p => p.Devoluciones).HasForeignKey(d => d.PrestamoId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Devolucion>().HasOne(d => d.Estado).WithMany(e => e.Devoluciones).HasForeignKey(d => d.EstadoId);
            modelBuilder.Entity<Devolucion>().HasOne(d => d.Usuario).WithMany(e => e.Devoluciones).HasForeignKey(d => d.UsuarioId).OnDelete(DeleteBehavior.NoAction);

            // Configuración de la tabla Multa
            modelBuilder.Entity<Multa>().HasOne(m => m.Usuario).WithMany(u => u.Multas).HasForeignKey(m => m.UsuarioId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Multa>().HasOne(m => m.Estado).WithMany(e => e.Multas).HasForeignKey(m => m.EstadoId);

            // Configuración de la tabla Notificaciones
            modelBuilder.Entity<Notificaciones>().HasKey(n => n.Id);

            //modelBuilder.Entity<Prestamo>().HasOne(m => m.Libro).WithMany(u => u.Prestamos).HasForeignKey(m => m.LibroId).OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Prestamo>().HasOne(m => m.Usuario).WithMany(u => u.Prestamos).HasForeignKey(m => m.UsuarioId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.Prestamos)
                      .HasForeignKey(p => p.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public async Task<bool> LoginUsuario(string Usuario, string password)
        {
            var Exitos = new SqlParameter("@Exitos", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await Database.ExecuteSqlRawAsync("EXEC sp_Login @User, @Contraseña, @Exitos OUTPUT",
                new SqlParameter("@User", Usuario ?? (object)DBNull.Value),
                new SqlParameter("@Contraseña", password ?? (object)DBNull.Value),
                Exitos);

            return (bool)(Exitos.Value ?? false);
        }

        // Me puede salir un usuario nulo y por ende es imporante el ? para que no me de error
        public async Task<Usuario?> ObtenerUsuario(string Usu, string password)
        {
            // Lo primero es recibir lo que el metodo en el sql me regresa
            var LUsuario = await Usuario.FromSqlRaw("sp_ObtenerUsuario @User, @Contraseña",
                new SqlParameter("@User", Usu),
                new SqlParameter("@Contraseña", password)).ToListAsync();

            // Me devuelve un usuario o un usuario nulo
            return LUsuario.FirstOrDefault();


        }

    }
}
