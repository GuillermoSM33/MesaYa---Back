﻿// <auto-generated />
using System;
using MesaYa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MesaYa.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MesaYa.Models.Auditoria", b =>
                {
                    b.Property<int>("AuditoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuditoriaId"));

                    b.Property<string>("Accion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Detalle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaEvento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("AuditoriaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Auditorias");

                    b.HasData(
                        new
                        {
                            AuditoriaId = 1,
                            Accion = "Insertar",
                            Detalle = "Detalle 1",
                            FechaEvento = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Module = "Reservas",
                            UsuarioId = 1
                        },
                        new
                        {
                            AuditoriaId = 2,
                            Accion = "Actualizar",
                            Detalle = "Detalle 2",
                            FechaEvento = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Module = "Menu",
                            UsuarioId = 2
                        },
                        new
                        {
                            AuditoriaId = 3,
                            Accion = "Eliminar",
                            Detalle = "Detalle 3",
                            FechaEvento = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Module = "Notificaciones",
                            UsuarioId = 3
                        });
                });

            modelBuilder.Entity("MesaYa.Models.ConfiguracionSistema", b =>
                {
                    b.Property<int>("ConfigId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConfigId"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Settings")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ConfigId");

                    b.ToTable("ConfiguracionesSistema");

                    b.HasData(
                        new
                        {
                            ConfigId = 1,
                            IsActive = true,
                            ModuleName = "Reservas",
                            Settings = "{\"key\":\"value1\"}",
                            UpdatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ConfigId = 2,
                            IsActive = true,
                            ModuleName = "Menu",
                            Settings = "{\"key\":\"value2\"}",
                            UpdatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ConfigId = 3,
                            IsActive = true,
                            ModuleName = "Notificaciones",
                            Settings = "{\"key\":\"value3\"}",
                            UpdatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            ConfigId = 4,
                            IsActive = true,
                            ModuleName = "Auditoria",
                            Settings = "{\"key\":\"value4\"}",
                            UpdatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("MesaYa.Models.ErrorSistema", b =>
                {
                    b.Property<int>("ErrorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ErrorId"));

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("FechaError")
                        .HasColumnType("datetime2");

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Resuelto")
                        .HasColumnType("bit");

                    b.Property<string>("StackTrace")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ErrorId");

                    b.ToTable("ErroresSistema");

                    b.HasData(
                        new
                        {
                            ErrorId = 1,
                            ErrorMessage = "Error 1",
                            FechaError = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Module = "Reservas",
                            Resuelto = false,
                            StackTrace = "StackTrace 1"
                        },
                        new
                        {
                            ErrorId = 2,
                            ErrorMessage = "Error 2",
                            FechaError = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Module = "Menu",
                            Resuelto = false,
                            StackTrace = "StackTrace 2"
                        },
                        new
                        {
                            ErrorId = 3,
                            ErrorMessage = "Error 3",
                            FechaError = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Module = "Notificaciones",
                            Resuelto = false,
                            StackTrace = "StackTrace 3"
                        },
                        new
                        {
                            ErrorId = 4,
                            ErrorMessage = "Error 4",
                            FechaError = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Module = "Auditoria",
                            Resuelto = false,
                            StackTrace = "StackTrace 4"
                        });
                });

            modelBuilder.Entity("MesaYa.Models.ItemAsRestaurante", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("RestauranteId")
                        .HasColumnType("int");

                    b.HasKey("ItemId", "RestauranteId");

                    b.HasIndex("RestauranteId");

                    b.ToTable("ItemAsRestaurantes");
                });

            modelBuilder.Entity("MesaYa.Models.MenuCategoria", b =>
                {
                    b.Property<int>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoriaId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoriaId");

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.ToTable("MenuCategorias");

                    b.HasData(
                        new
                        {
                            CategoriaId = 1,
                            Nombre = "Entradas"
                        },
                        new
                        {
                            CategoriaId = 2,
                            Nombre = "Platos Principales"
                        },
                        new
                        {
                            CategoriaId = 3,
                            Nombre = "Postres"
                        },
                        new
                        {
                            CategoriaId = 4,
                            Nombre = "Bebidas"
                        });
                });

            modelBuilder.Entity("MesaYa.Models.MenuItem", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemId"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Disponible")
                        .HasColumnType("bit");

                    b.Property<string>("ImagenUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("ItemId");

                    b.HasIndex("CategoriaId");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new
                        {
                            ItemId = 1,
                            CategoriaId = 1,
                            Descripcion = "Ensalada con pollo",
                            Disponible = true,
                            ImagenUrl = "url1",
                            IsDeleted = false,
                            Nombre = "Ensalada César",
                            Precio = 9.99m
                        },
                        new
                        {
                            ItemId = 2,
                            CategoriaId = 2,
                            Descripcion = "Filete con papas",
                            Disponible = true,
                            ImagenUrl = "url2",
                            IsDeleted = false,
                            Nombre = "Filete de Res",
                            Precio = 19.99m
                        },
                        new
                        {
                            ItemId = 3,
                            CategoriaId = 3,
                            Descripcion = "Postre de queso",
                            Disponible = true,
                            ImagenUrl = "url3",
                            IsDeleted = false,
                            Nombre = "Cheesecake",
                            Precio = 7.99m
                        },
                        new
                        {
                            ItemId = 4,
                            CategoriaId = 4,
                            Descripcion = "Bebida refrescante",
                            Disponible = true,
                            ImagenUrl = "url4",
                            IsDeleted = false,
                            Nombre = "Limonada",
                            Precio = 3.99m
                        });
                });

            modelBuilder.Entity("MesaYa.Models.Mesa", b =>
                {
                    b.Property<int>("MesaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MesaId"));

                    b.Property<int>("Capacidad")
                        .HasColumnType("int");

                    b.Property<bool>("Disponible")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MesaNumero")
                        .HasColumnType("int");

                    b.Property<int>("RestauranteId")
                        .HasColumnType("int");

                    b.HasKey("MesaId");

                    b.HasIndex("RestauranteId");

                    b.ToTable("Mesa");

                    b.HasData(
                        new
                        {
                            MesaId = 1,
                            Capacidad = 4,
                            Disponible = true,
                            IsDeleted = false,
                            MesaNumero = 0,
                            RestauranteId = 1
                        },
                        new
                        {
                            MesaId = 2,
                            Capacidad = 6,
                            Disponible = true,
                            IsDeleted = false,
                            MesaNumero = 0,
                            RestauranteId = 2
                        },
                        new
                        {
                            MesaId = 3,
                            Capacidad = 2,
                            Disponible = true,
                            IsDeleted = false,
                            MesaNumero = 0,
                            RestauranteId = 2
                        });
                });

            modelBuilder.Entity("MesaYa.Models.Notificacion", b =>
                {
                    b.Property<int>("NotificacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificacionId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enviado")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaEnvio")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mensaje")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("NotificacionId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Notificaciones");

                    b.HasData(
                        new
                        {
                            NotificacionId = 1,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Enviado = false,
                            Mensaje = "Mensaje 1",
                            Tipo = "email",
                            UsuarioId = 1
                        },
                        new
                        {
                            NotificacionId = 2,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Enviado = false,
                            Mensaje = "Mensaje 2",
                            Tipo = "sms",
                            UsuarioId = 2
                        },
                        new
                        {
                            NotificacionId = 3,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Enviado = false,
                            Mensaje = "Mensaje 3",
                            Tipo = "push",
                            UsuarioId = 3
                        });
                });

            modelBuilder.Entity("MesaYa.Models.Reserva", b =>
                {
                    b.Property<int>("ReservaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservaId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("FechaReserva")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MesaId")
                        .HasColumnType("int");

                    b.Property<int>("NumeroPersonas")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("ReservaId");

                    b.HasIndex("MesaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Reservas");

                    b.HasData(
                        new
                        {
                            ReservaId = 1,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Estado = "pendiente",
                            FechaReserva = new DateTime(2025, 3, 9, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            MesaId = 1,
                            NumeroPersonas = 2,
                            UsuarioId = 1
                        },
                        new
                        {
                            ReservaId = 2,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Estado = "confirmada",
                            FechaReserva = new DateTime(2025, 3, 10, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            MesaId = 2,
                            NumeroPersonas = 4,
                            UsuarioId = 2
                        },
                        new
                        {
                            ReservaId = 3,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Estado = "cancelada",
                            FechaReserva = new DateTime(2025, 3, 11, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            MesaId = 3,
                            NumeroPersonas = 3,
                            UsuarioId = 3
                        });
                });

            modelBuilder.Entity("MesaYa.Models.ReservaAsMesa", b =>
                {
                    b.Property<int>("ReservaId")
                        .HasColumnType("int");

                    b.Property<int>("MesaId")
                        .HasColumnType("int");

                    b.HasKey("ReservaId", "MesaId");

                    b.HasIndex("MesaId");

                    b.ToTable("ReservaAsMesas");
                });

            modelBuilder.Entity("MesaYa.Models.Restaurante", b =>
                {
                    b.Property<int>("RestauranteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RestauranteId"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Horario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("RestauranteNombre")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("RestauranteId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Restaurantes");

                    b.HasData(
                        new
                        {
                            RestauranteId = 1,
                            Descripcion = "Este reastureante sabe bien",
                            Direccion = "Calle 1",
                            Horario = "sadsd",
                            ImagenUrl = "Imagenreal",
                            IsDeleted = false,
                            RestauranteNombre = "Restaurante 1",
                            Telefono = "1234567890",
                            UsuarioId = 4
                        },
                        new
                        {
                            RestauranteId = 2,
                            Descripcion = "Este reastureante sabe bien",
                            Direccion = "Calle 2",
                            Horario = "sadsd",
                            ImagenUrl = "Imagenreal",
                            IsDeleted = false,
                            RestauranteNombre = "Restaurante 2",
                            Telefono = "0987654321",
                            UsuarioId = 2
                        });
                });

            modelBuilder.Entity("MesaYa.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            Description = "Administrador del sistema",
                            RoleName = "Admin"
                        },
                        new
                        {
                            RoleId = 2,
                            Description = "Usuarios Finales",
                            RoleName = "Usuario"
                        },
                        new
                        {
                            RoleId = 3,
                            Description = "Encargado de llevar el control del resturante",
                            RoleName = "Hostess"
                        });
                });

            modelBuilder.Entity("MesaYa.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UsuarioId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            UsuarioId = 1,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "guillermo.jesus.garcia.canul@gmail.com",
                            IsDeleted = false,
                            PasswordHash = "hash1",
                            Username = "Guillermo Garcia Canul"
                        },
                        new
                        {
                            UsuarioId = 2,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "alissonmedcan@gmail.com",
                            IsDeleted = false,
                            PasswordHash = "hash2",
                            Username = "Alisson Alexandra Medina Canche"
                        },
                        new
                        {
                            UsuarioId = 3,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ayshagama@gmail.com",
                            IsDeleted = false,
                            PasswordHash = "hash3",
                            Username = "Aysha Garcia Medina"
                        },
                        new
                        {
                            UsuarioId = 4,
                            CreatedAt = new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "lionel@gmail.com",
                            IsDeleted = false,
                            PasswordHash = "hash4",
                            Username = "Lionel Andres Messi"
                        });
                });

            modelBuilder.Entity("MesaYa.Models.UsuarioAsRole", b =>
                {
                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UsuarioId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UsuarioAsRoles");

                    b.HasData(
                        new
                        {
                            UsuarioId = 1,
                            RoleId = 1
                        },
                        new
                        {
                            UsuarioId = 2,
                            RoleId = 3
                        },
                        new
                        {
                            UsuarioId = 3,
                            RoleId = 2
                        },
                        new
                        {
                            UsuarioId = 4,
                            RoleId = 3
                        });
                });

            modelBuilder.Entity("MesaYa.Models.Auditoria", b =>
                {
                    b.HasOne("MesaYa.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MesaYa.Models.ItemAsRestaurante", b =>
                {
                    b.HasOne("MesaYa.Models.MenuItem", "MenuItem")
                        .WithMany("ItemAsRestaurantes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MesaYa.Models.Restaurante", "Restaurante")
                        .WithMany("ItemAsRestaurantes")
                        .HasForeignKey("RestauranteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuItem");

                    b.Navigation("Restaurante");
                });

            modelBuilder.Entity("MesaYa.Models.MenuItem", b =>
                {
                    b.HasOne("MesaYa.Models.MenuCategoria", "MenuCategoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuCategoria");
                });

            modelBuilder.Entity("MesaYa.Models.Mesa", b =>
                {
                    b.HasOne("MesaYa.Models.Restaurante", "Restaurante")
                        .WithMany()
                        .HasForeignKey("RestauranteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurante");
                });

            modelBuilder.Entity("MesaYa.Models.Notificacion", b =>
                {
                    b.HasOne("MesaYa.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MesaYa.Models.Reserva", b =>
                {
                    b.HasOne("MesaYa.Models.Mesa", "Mesa")
                        .WithMany()
                        .HasForeignKey("MesaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MesaYa.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Mesa");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MesaYa.Models.ReservaAsMesa", b =>
                {
                    b.HasOne("MesaYa.Models.Mesa", "Mesa")
                        .WithMany("ReservaAsMesas")
                        .HasForeignKey("MesaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MesaYa.Models.Reserva", "Reserva")
                        .WithMany("ReservaAsMesas")
                        .HasForeignKey("ReservaId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Mesa");

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("MesaYa.Models.Restaurante", b =>
                {
                    b.HasOne("MesaYa.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MesaYa.Models.UsuarioAsRole", b =>
                {
                    b.HasOne("MesaYa.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MesaYa.Models.Usuario", "Usuario")
                        .WithMany("UsuarioAsRoles")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MesaYa.Models.MenuItem", b =>
                {
                    b.Navigation("ItemAsRestaurantes");
                });

            modelBuilder.Entity("MesaYa.Models.Mesa", b =>
                {
                    b.Navigation("ReservaAsMesas");
                });

            modelBuilder.Entity("MesaYa.Models.Reserva", b =>
                {
                    b.Navigation("ReservaAsMesas");
                });

            modelBuilder.Entity("MesaYa.Models.Restaurante", b =>
                {
                    b.Navigation("ItemAsRestaurantes");
                });

            modelBuilder.Entity("MesaYa.Models.Usuario", b =>
                {
                    b.Navigation("UsuarioAsRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
