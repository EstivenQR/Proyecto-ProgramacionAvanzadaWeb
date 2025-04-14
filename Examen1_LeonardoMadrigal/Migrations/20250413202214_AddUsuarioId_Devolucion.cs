using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen1_LeonardoMadrigal.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioId_Devolucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Devolucion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devolucion_UsuarioId",
                table: "Devolucion",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devolucion_Usuario_UsuarioId",
                table: "Devolucion",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devolucion_Usuario_UsuarioId",
                table: "Devolucion");

            migrationBuilder.DropIndex(
                name: "IX_Devolucion_UsuarioId",
                table: "Devolucion");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Devolucion");
        }
    }
}
