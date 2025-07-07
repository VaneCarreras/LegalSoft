using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class localidad2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalidadID",
                table: "Personas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_LocalidadID",
                table: "Personas",
                column: "LocalidadID");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Localidades_LocalidadID",
                table: "Personas",
                column: "LocalidadID",
                principalTable: "Localidades",
                principalColumn: "LocalidadID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Localidades_LocalidadID",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_LocalidadID",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "LocalidadID",
                table: "Personas");
        }
    }
}
