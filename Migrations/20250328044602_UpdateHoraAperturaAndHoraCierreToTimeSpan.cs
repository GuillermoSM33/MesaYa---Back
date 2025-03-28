using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MesaYa.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHoraAperturaAndHoraCierreToTimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracionesSistema",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesSistema", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "ErroresSistema",
                columns: table => new
                {
                    ErrorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaError = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Resuelto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErroresSistema", x => x.ErrorId);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "RevokedTokens",
                columns: table => new
                {
                    Id_Token = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokedTokens", x => x.Id_Token);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuCategorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "MenuCategorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    AuditoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEvento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.AuditoriaId);
                    table.ForeignKey(
                        name: "FK_Auditorias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    NotificacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Enviado = table.Column<bool>(type: "bit", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.NotificacionId);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    ReservaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NumeroPersonas = table.Column<int>(type: "int", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.ReservaId);
                    table.ForeignKey(
                        name: "FK_Reservas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "Restaurantes",
                columns: table => new
                {
                    RestauranteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    RestauranteNombre = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraApertura = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraCierre = table.Column<TimeSpan>(type: "time", nullable: false),
                    Horario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurantes", x => x.RestauranteId);
                    table.ForeignKey(
                        name: "FK_Restaurantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioAsRoles",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioAsRoles", x => new { x.UsuarioId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UsuarioAsRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioAsRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemAsRestaurantes",
                columns: table => new
                {
                    RestauranteId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAsRestaurantes", x => new { x.ItemId, x.RestauranteId });
                    table.ForeignKey(
                        name: "FK_ItemAsRestaurantes_MenuItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MenuItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemAsRestaurantes_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "RestauranteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mesa",
                columns: table => new
                {
                    MesaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MesaNumero = table.Column<int>(type: "int", nullable: false),
                    RestauranteId = table.Column<int>(type: "int", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesa", x => x.MesaId);
                    table.ForeignKey(
                        name: "FK_Mesa_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "RestauranteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantesFavoritos",
                columns: table => new
                {
                    RestauranteId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantesFavoritos", x => new { x.UsuarioId, x.RestauranteId });
                    table.ForeignKey(
                        name: "FK_RestaurantesFavoritos_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "RestauranteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantesFavoritos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "ReservaAsMesas",
                columns: table => new
                {
                    ReservaId = table.Column<int>(type: "int", nullable: false),
                    MesaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaAsMesas", x => new { x.ReservaId, x.MesaId });
                    table.ForeignKey(
                        name: "FK_ReservaAsMesas_Mesa_MesaId",
                        column: x => x.MesaId,
                        principalTable: "Mesa",
                        principalColumn: "MesaId");
                    table.ForeignKey(
                        name: "FK_ReservaAsMesas_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "ReservaId");
                });

            migrationBuilder.InsertData(
                table: "ConfiguracionesSistema",
                columns: new[] { "ConfigId", "IsActive", "ModuleName", "Settings", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, true, "Reservas", "{\"key\":\"value1\"}", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, true, "Menu", "{\"key\":\"value2\"}", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, true, "Notificaciones", "{\"key\":\"value3\"}", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, true, "Auditoria", "{\"key\":\"value4\"}", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ErroresSistema",
                columns: new[] { "ErrorId", "ErrorMessage", "FechaError", "Module", "Resuelto", "StackTrace" },
                values: new object[,]
                {
                    { 1, "Error 1", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Reservas", false, "StackTrace 1" },
                    { 2, "Error 2", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Menu", false, "StackTrace 2" },
                    { 3, "Error 3", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Notificaciones", false, "StackTrace 3" },
                    { 4, "Error 4", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Auditoria", false, "StackTrace 4" }
                });

            migrationBuilder.InsertData(
                table: "MenuCategorias",
                columns: new[] { "CategoriaId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Entradas" },
                    { 2, "Platos Principales" },
                    { 3, "Postres" },
                    { 4, "Bebidas" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleName" },
                values: new object[,]
                {
                    { 1, "Administrador del sistema", "Admin" },
                    { 2, "Usuarios Finales", "Usuario" },
                    { 3, "Encargado de llevar el control del resturante", "Hostess" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "UsuarioId", "CreatedAt", "Email", "IsDeleted", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "guillermo.jesus.garcia.canul@gmail.com", false, "hash1", "Guillermo Garcia Canul" },
                    { 2, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "alissonmedcan@gmail.com", false, "hash2", "Alisson Alexandra Medina Canche" },
                    { 3, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "ayshagama@gmail.com", false, "hash3", "Aysha Garcia Medina" },
                    { 4, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "lionel@gmail.com", false, "hash4", "Lionel Andres Messi" }
                });

            migrationBuilder.InsertData(
                table: "Auditorias",
                columns: new[] { "AuditoriaId", "Accion", "Detalle", "FechaEvento", "Module", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "Insertar", "Detalle 1", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Reservas", 1 },
                    { 2, "Actualizar", "Detalle 2", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Menu", 2 },
                    { 3, "Eliminar", "Detalle 3", new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Notificaciones", 3 }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "ItemId", "CategoriaId", "Descripcion", "Disponible", "ImagenUrl", "IsDeleted", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, 1, "Ensalada con pollo", true, "url1", false, "Ensalada César", 9.99m },
                    { 2, 2, "Filete con papas", true, "url2", false, "Filete de Res", 19.99m },
                    { 3, 3, "Postre de queso", true, "url3", false, "Cheesecake", 7.99m },
                    { 4, 4, "Bebida refrescante", true, "url4", false, "Limonada", 3.99m }
                });

            migrationBuilder.InsertData(
                table: "Notificaciones",
                columns: new[] { "NotificacionId", "CreatedAt", "Enviado", "FechaEnvio", "Mensaje", "Tipo", "UsuarioId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Mensaje 1", "email", 1 },
                    { 2, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Mensaje 2", "sms", 2 },
                    { 3, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Mensaje 3", "push", 3 }
                });

            migrationBuilder.InsertData(
                table: "Reservas",
                columns: new[] { "ReservaId", "CreatedAt", "Estado", "FechaReserva", "HoraFin", "IsDeleted", "NumeroPersonas", "UsuarioId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "pendiente", new DateTime(2025, 3, 9, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 1 },
                    { 2, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "confirmada", new DateTime(2025, 3, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4, 2 },
                    { 3, new DateTime(2025, 3, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "cancelada", new DateTime(2025, 3, 11, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Restaurantes",
                columns: new[] { "RestauranteId", "Descripcion", "Direccion", "HoraApertura", "HoraCierre", "Horario", "ImagenUrl", "IsDeleted", "RestauranteNombre", "Telefono", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "Este restaurante sabe bien", "Calle 1", new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0), "Tarde-Noche", "Imagenreal", false, "Restaurante 1", "1234567890", 4 },
                    { 2, "Este restaurante sabe bien", "Calle 2", new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0), "Tarde-Noche", "Imagenreal", false, "Restaurante 2", "0987654321", 2 }
                });

            migrationBuilder.InsertData(
                table: "UsuarioAsRoles",
                columns: new[] { "RoleId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 3, 2 },
                    { 2, 3 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "Mesa",
                columns: new[] { "MesaId", "Capacidad", "Disponible", "IsDeleted", "MesaNumero", "RestauranteId" },
                values: new object[,]
                {
                    { 1, 4, true, false, 0, 1 },
                    { 2, 6, true, false, 0, 2 },
                    { 3, 2, true, false, 0, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_UsuarioId",
                table: "Auditorias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAsRestaurantes_RestauranteId",
                table: "ItemAsRestaurantes",
                column: "RestauranteId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategorias_Nombre",
                table: "MenuCategorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoriaId",
                table: "MenuItems",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesa_RestauranteId",
                table: "Mesa",
                column: "RestauranteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioId",
                table: "Notificaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaAsMesas_MesaId",
                table: "ReservaAsMesas",
                column: "MesaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_UsuarioId",
                table: "Reservas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurantes_UsuarioId",
                table: "Restaurantes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantesFavoritos_RestauranteId",
                table: "RestaurantesFavoritos",
                column: "RestauranteId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioAsRoles_RoleId",
                table: "UsuarioAsRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "ConfiguracionesSistema");

            migrationBuilder.DropTable(
                name: "ErroresSistema");

            migrationBuilder.DropTable(
                name: "ItemAsRestaurantes");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "ReservaAsMesas");

            migrationBuilder.DropTable(
                name: "RestaurantesFavoritos");

            migrationBuilder.DropTable(
                name: "RevokedTokens");

            migrationBuilder.DropTable(
                name: "UsuarioAsRoles");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Mesa");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "MenuCategorias");

            migrationBuilder.DropTable(
                name: "Restaurantes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
