using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen1_LeonardoMadrigal.Migrations
{
    /// <inheritdoc />
    public partial class CambioTablaDevolucion_Y_Prestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devolucion_Pedido_PedidoId",
                table: "Devolucion");

            migrationBuilder.RenameColumn(
                name: "PedidoId",
                table: "Devolucion",
                newName: "PrestamoId");

            migrationBuilder.RenameIndex(
                name: "IX_Devolucion_PedidoId",
                table: "Devolucion",
                newName: "IX_Devolucion_PrestamoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devolucion_Prestamo_PrestamoId",
                table: "Devolucion",
                column: "PrestamoId",
                principalTable: "Prestamo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devolucion_Prestamo_PrestamoId",
                table: "Devolucion");

            migrationBuilder.RenameColumn(
                name: "PrestamoId",
                table: "Devolucion",
                newName: "PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_Devolucion_PrestamoId",
                table: "Devolucion",
                newName: "IX_Devolucion_PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devolucion_Pedido_PedidoId",
                table: "Devolucion",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id");
        }
    }
}
