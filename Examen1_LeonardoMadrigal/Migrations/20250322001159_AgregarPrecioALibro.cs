using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen1_LeonardoMadrigal.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPrecioALibro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Libro",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Libro");
        }
    }
}
