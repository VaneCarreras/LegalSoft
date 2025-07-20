using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class contactoCampos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mensaje",
                table: "Contactos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mensaje",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
