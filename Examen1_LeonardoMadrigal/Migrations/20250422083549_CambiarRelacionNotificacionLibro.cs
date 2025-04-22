using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen1_LeonardoMadrigal.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionNotificacionLibro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libro_Notificacion_NotificacionId",
                table: "Libro");

            migrationBuilder.DropIndex(
                name: "IX_Libro_NotificacionId",
                table: "Libro");

            migrationBuilder.DropColumn(
                name: "NotificacionId",
                table: "Libro");

            migrationBuilder.AlterColumn<string>(
                name: "Mensaje",
                table: "Notificacion",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "LibroId",
                table: "Notificacion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_LibroId",
                table: "Notificacion",
                column: "LibroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacion_Libro_LibroId",
                table: "Notificacion",
                column: "LibroId",
                principalTable: "Libro",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacion_Libro_LibroId",
                table: "Notificacion");

            migrationBuilder.DropIndex(
                name: "IX_Notificacion_LibroId",
                table: "Notificacion");

            migrationBuilder.DropColumn(
                name: "LibroId",
                table: "Notificacion");

            migrationBuilder.AlterColumn<string>(
                name: "Mensaje",
                table: "Notificacion",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "NotificacionId",
                table: "Libro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Libro_NotificacionId",
                table: "Libro",
                column: "NotificacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libro_Notificacion_NotificacionId",
                table: "Libro",
                column: "NotificacionId",
                principalTable: "Notificacion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
