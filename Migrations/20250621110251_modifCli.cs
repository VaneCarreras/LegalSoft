using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class modifCli : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteID",
                table: "Documentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ClienteID",
                table: "Documentos",
                column: "ClienteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Clientes_ClienteID",
                table: "Documentos",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Clientes_ClienteID",
                table: "Documentos");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_ClienteID",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "ClienteID",
                table: "Documentos");
        }
    }
}
