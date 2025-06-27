using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class modifConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoConsulta",
                table: "Consultas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoConsulta",
                table: "Consultas");
        }
    }
}
