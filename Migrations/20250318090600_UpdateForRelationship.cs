using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesaYa.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Mesa_MesaId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_MesaId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "MesaId",
                table: "Reservas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MesaId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Reservas",
                keyColumn: "ReservaId",
                keyValue: 1,
                column: "MesaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Reservas",
                keyColumn: "ReservaId",
                keyValue: 2,
                column: "MesaId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Reservas",
                keyColumn: "ReservaId",
                keyValue: 3,
                column: "MesaId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_MesaId",
                table: "Reservas",
                column: "MesaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Mesa_MesaId",
                table: "Reservas",
                column: "MesaId",
                principalTable: "Mesa",
                principalColumn: "MesaId");
        }
    }
}
