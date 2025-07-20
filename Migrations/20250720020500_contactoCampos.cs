using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class contactoCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaSeleccionada",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Datos",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Datos2",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Delito",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detalle",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detalle2",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Empleador",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Horas",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Situacion",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sueldo",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vinculo",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaSeleccionada",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Datos",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Datos2",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Delito",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Detalle",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Detalle2",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Empleador",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Horas",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Situacion",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Sueldo",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Vinculo",
                table: "Contactos");
        }
    }
}
