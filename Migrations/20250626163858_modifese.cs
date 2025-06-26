using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class modifese : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagenClientes_Clientes_ClienteID",
                table: "ImagenClientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImagenClientes",
                table: "ImagenClientes");

            migrationBuilder.DropColumn(
                name: "NombreArchivo",
                table: "ImagenClientes");

            migrationBuilder.DropColumn(
                name: "TipoImg",
                table: "ImagenClientes");

            migrationBuilder.RenameTable(
                name: "ImagenClientes",
                newName: "ImagenCliente");

            migrationBuilder.RenameIndex(
                name: "IX_ImagenClientes_ClienteID",
                table: "ImagenCliente",
                newName: "IX_ImagenCliente_ClienteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImagenCliente",
                table: "ImagenCliente",
                column: "ImagenClienteID");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagenCliente_Clientes_ClienteID",
                table: "ImagenCliente",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagenCliente_Clientes_ClienteID",
                table: "ImagenCliente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImagenCliente",
                table: "ImagenCliente");

            migrationBuilder.RenameTable(
                name: "ImagenCliente",
                newName: "ImagenClientes");

            migrationBuilder.RenameIndex(
                name: "IX_ImagenCliente_ClienteID",
                table: "ImagenClientes",
                newName: "IX_ImagenClientes_ClienteID");

            migrationBuilder.AddColumn<string>(
                name: "NombreArchivo",
                table: "ImagenClientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoImg",
                table: "ImagenClientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImagenClientes",
                table: "ImagenClientes",
                column: "ImagenClienteID");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagenClientes_Clientes_ClienteID",
                table: "ImagenClientes",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
