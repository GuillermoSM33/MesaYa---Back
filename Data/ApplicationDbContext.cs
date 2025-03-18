using Microsoft.EntityFrameworkCore;
using MesaYa.Models;


namespace MesaYa.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioAsRole> UsuarioAsRoles { get; set; }
        public DbSet<Restaurante> Restaurantes { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<MenuCategoria> MenuCategorias { get; set; }
        public DbSet<ItemAsRestaurante> ItemAsRestaurantes { get; set; }
        public DbSet<ReservaAsMesa> ReservaAsMesas { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<ErrorSistema> ErroresSistema { get; set; }
        public DbSet<ConfiguracionSistema> ConfiguracionesSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de clave primaria compuesta para UsuarioAsRole
            modelBuilder.Entity<UsuarioAsRole>()
                .HasKey(ur => new { ur.UsuarioId, ur.RoleId });

            // Restricción de unicidad en RoleName de Role
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // Restricción de unicidad en Username y Email de Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Restricción de unicidad en Nombre de MenuCategoria
            modelBuilder.Entity<MenuCategoria>()
                .HasIndex(mc => mc.Nombre)
                .IsUnique();

            // Configuración para la propiedad decimal Precio en MenuItem
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Precio)
                .HasColumnType("decimal(10,2)");

            // Clave compuesta para ReservaAsMesa
            modelBuilder.Entity<ReservaAsMesa>()
                .HasKey(rm => new { rm.ReservaId, rm.MesaId });

            // Configurar la relación muchos-a-muchos entre Reserva y Mesa
            modelBuilder.Entity<ReservaAsMesa>()
                .HasOne(rm => rm.Reserva)
                .WithMany(r => r.ReservaAsMesas)
                .HasForeignKey(rm => rm.ReservaId)
                .OnDelete(DeleteBehavior.NoAction);  // Evita ciclos de cascada

            modelBuilder.Entity<ReservaAsMesa>()
                .HasOne(rm => rm.Mesa)
                .WithMany(m => m.ReservaAsMesas)
                .HasForeignKey(rm => rm.MesaId)
                .OnDelete(DeleteBehavior.NoAction);  // Evita eliminación en cascada

            // Configurar la relación de Reserva con Usuario (Evitar CASCADE)
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);  // Soft delete

            modelBuilder.Entity<ItemAsRestaurante>()
                .HasKey(ir => new { ir.ItemId, ir.RestauranteId });

            modelBuilder.Entity<ItemAsRestaurante>()
                .HasOne(ir => ir.MenuItem)
                .WithMany(i => i.ItemAsRestaurantes)
                .HasForeignKey(ir => ir.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemAsRestaurante>()
                .HasOne(ir => ir.Restaurante)
                .WithMany(r => r.ItemAsRestaurantes)
                .HasForeignKey(ir => ir.RestauranteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Fecha base para el seed data (valor constante para evitar problemas en migraciones)
            var baseDate = new DateTime(2025, 3, 8, 12, 0, 0);

            // Seed para Roles: 3 registros
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Administrador del sistema" },
                new Role { RoleId = 2, RoleName = "Usuario", Description = "Usuarios Finales" },
                new Role { RoleId = 3, RoleName = "Hostess", Description = "Encargado de llevar el control del resturante" }
            );

            // Seed para Usuarios: 3 registros
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { UsuarioId = 1, Username = "Guillermo Garcia Canul", Email = "guillermo.jesus.garcia.canul@gmail.com", PasswordHash = "hash1", CreatedAt = baseDate, IsDeleted = false },
                new Usuario { UsuarioId = 2, Username = "Alisson Alexandra Medina Canche", Email = "alissonmedcan@gmail.com", PasswordHash = "hash2", CreatedAt = baseDate, IsDeleted = false },
                new Usuario { UsuarioId = 3, Username = "Aysha Garcia Medina", Email = "ayshagama@gmail.com", PasswordHash = "hash3", CreatedAt = baseDate, IsDeleted = false },
                new Usuario { UsuarioId = 4, Username= "Lionel Andres Messi", Email="lionel@gmail.com", PasswordHash = "hash4", CreatedAt = baseDate, IsDeleted = false }
                );

            // Seed para UsuarioAsRoles: se asigna un rol a cada usuario
            modelBuilder.Entity<UsuarioAsRole>().HasData(
                new UsuarioAsRole { UsuarioId = 1, RoleId = 1 },
                new UsuarioAsRole { UsuarioId = 2, RoleId = 3 },
                new UsuarioAsRole { UsuarioId = 3, RoleId = 2 },
                new UsuarioAsRole { UsuarioId = 4, RoleId = 3 }
            );

            // Seed para MenuCategorias: 4 registros
            modelBuilder.Entity<MenuCategoria>().HasData(
                new MenuCategoria { CategoriaId = 1, Nombre = "Entradas" },
                new MenuCategoria { CategoriaId = 2, Nombre = "Platos Principales" },
                new MenuCategoria { CategoriaId = 3, Nombre = "Postres" },
                new MenuCategoria { CategoriaId = 4, Nombre = "Bebidas" }
            );

            // Seed para Restaurantes: 2 registros
            modelBuilder.Entity<Restaurante>().HasData(
                new Restaurante { RestauranteId = 1,UsuarioId = 4, RestauranteNombre = "Restaurante 1", Direccion = "Calle 1", Telefono = "1234567890",ImagenUrl="Imagenreal", Horario="sadsd",Descripcion= "Este reastureante sabe bien", IsDeleted = false },
                new Restaurante { RestauranteId = 2,UsuarioId = 2, RestauranteNombre = "Restaurante 2", Direccion = "Calle 2", Telefono = "0987654321", ImagenUrl = "Imagenreal", Horario = "sadsd", Descripcion = "Este reastureante sabe bien", IsDeleted = false }
            );

            //seed para Mesas: 3 registros
            modelBuilder.Entity<Mesa>().HasData(
                new Mesa { MesaId = 1, RestauranteId = 1, Capacidad = 4,Disponible= true,  IsDeleted = false },
                new Mesa { MesaId = 2, RestauranteId = 2, Capacidad = 6, Disponible=true, IsDeleted = false },
                new Mesa { MesaId = 3, RestauranteId = 2, Capacidad = 2, Disponible = true, IsDeleted = false }
                );

            // Seed para Reservas: 3 registros con diferentes fechas de reserva
            modelBuilder.Entity<Reserva>().HasData(
                new Reserva { ReservaId = 1, UsuarioId = 1, FechaReserva = baseDate.AddDays(1), Estado = "pendiente", NumeroPersonas = 2, CreatedAt = baseDate, IsDeleted = false },
                new Reserva { ReservaId = 2, UsuarioId = 2, FechaReserva = baseDate.AddDays(2), Estado = "confirmada", NumeroPersonas = 4, CreatedAt = baseDate, IsDeleted = false },
                new Reserva { ReservaId = 3, UsuarioId = 3, FechaReserva = baseDate.AddDays(3), Estado = "cancelada", NumeroPersonas = 3, CreatedAt = baseDate, IsDeleted = false }
            );

            // Seed para MenuItems: 4 registros, cada uno asociado a una categoría
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { ItemId = 1, CategoriaId = 1, Nombre = "Ensalada César", Descripcion = "Ensalada con pollo", Precio = 9.99m, ImagenUrl = "url1", Disponible = true, IsDeleted = false },
                new MenuItem { ItemId = 2, CategoriaId = 2, Nombre = "Filete de Res", Descripcion = "Filete con papas", Precio = 19.99m, ImagenUrl = "url2", Disponible = true, IsDeleted = false },
                new MenuItem { ItemId = 3, CategoriaId = 3, Nombre = "Cheesecake", Descripcion = "Postre de queso", Precio = 7.99m, ImagenUrl = "url3", Disponible = true, IsDeleted = false },
                new MenuItem { ItemId = 4, CategoriaId = 4, Nombre = "Limonada", Descripcion = "Bebida refrescante", Precio = 3.99m, ImagenUrl = "url4", Disponible = true, IsDeleted = false }
            );

            // Seed para Notificaciones: 3 registros
            modelBuilder.Entity<Notificacion>().HasData(
                new Notificacion { NotificacionId = 1, UsuarioId = 1, Mensaje = "Mensaje 1", Tipo = "email", Enviado = false, FechaEnvio = null, CreatedAt = baseDate },
                new Notificacion { NotificacionId = 2, UsuarioId = 2, Mensaje = "Mensaje 2", Tipo = "sms", Enviado = false, FechaEnvio = null, CreatedAt = baseDate },
                new Notificacion { NotificacionId = 3, UsuarioId = 3, Mensaje = "Mensaje 3", Tipo = "push", Enviado = false, FechaEnvio = null, CreatedAt = baseDate }
            );

            // Seed para Auditorias: 3 registros
            modelBuilder.Entity<Auditoria>().HasData(
                new Auditoria { AuditoriaId = 1, UsuarioId = 1, Module = "Reservas", Accion = "Insertar", Detalle = "Detalle 1", FechaEvento = baseDate },
                new Auditoria { AuditoriaId = 2, UsuarioId = 2, Module = "Menu", Accion = "Actualizar", Detalle = "Detalle 2", FechaEvento = baseDate },
                new Auditoria { AuditoriaId = 3, UsuarioId = 3, Module = "Notificaciones", Accion = "Eliminar", Detalle = "Detalle 3", FechaEvento = baseDate }
            );

            // Seed para ErroresSistema: 4 registros
            modelBuilder.Entity<ErrorSistema>().HasData(
                new ErrorSistema { ErrorId = 1, Module = "Reservas", ErrorMessage = "Error 1", StackTrace = "StackTrace 1", FechaError = baseDate, Resuelto = false },
                new ErrorSistema { ErrorId = 2, Module = "Menu", ErrorMessage = "Error 2", StackTrace = "StackTrace 2", FechaError = baseDate, Resuelto = false },
                new ErrorSistema { ErrorId = 3, Module = "Notificaciones", ErrorMessage = "Error 3", StackTrace = "StackTrace 3", FechaError = baseDate, Resuelto = false },
                new ErrorSistema { ErrorId = 4, Module = "Auditoria", ErrorMessage = "Error 4", StackTrace = "StackTrace 4", FechaError = baseDate, Resuelto = false }
            );

            // Seed para ConfiguracionesSistema: 4 registros
            modelBuilder.Entity<ConfiguracionSistema>().HasData(
                new ConfiguracionSistema { ConfigId = 1, ModuleName = "Reservas", IsActive = true, Settings = "{\"key\":\"value1\"}", UpdatedAt = baseDate },
                new ConfiguracionSistema { ConfigId = 2, ModuleName = "Menu", IsActive = true, Settings = "{\"key\":\"value2\"}", UpdatedAt = baseDate },
                new ConfiguracionSistema { ConfigId = 3, ModuleName = "Notificaciones", IsActive = true, Settings = "{\"key\":\"value3\"}", UpdatedAt = baseDate },
                new ConfiguracionSistema { ConfigId = 4, ModuleName = "Auditoria", IsActive = true, Settings = "{\"key\":\"value4\"}", UpdatedAt = baseDate }
            );

            base.OnModelCreating(modelBuilder);
        }

    }
}
