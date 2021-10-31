using Microsoft.EntityFrameworkCore.Migrations;

namespace InventarioCruzRoja.Migrations
{
    public partial class AgregarCoordenadasSedes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Sedes",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Sedes",
                type: "double",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Sedes");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Sedes");
        }
    }
}
