using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class modifDocsExp1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocExpedientes_Expedientes_ExpedienteID",
                table: "DocExpedientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocExpedientes",
                table: "DocExpedientes");

            migrationBuilder.DropColumn(
                name: "Base64",
                table: "DocExpedientes");

            migrationBuilder.RenameTable(
                name: "DocExpedientes",
                newName: "DocsExpediente");

            migrationBuilder.RenameIndex(
                name: "IX_DocExpedientes_ExpedienteID",
                table: "DocsExpediente",
                newName: "IX_DocsExpediente_ExpedienteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocsExpediente",
                table: "DocsExpediente",
                column: "DocID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocsExpediente_Expedientes_ExpedienteID",
                table: "DocsExpediente",
                column: "ExpedienteID",
                principalTable: "Expedientes",
                principalColumn: "ExpedienteID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocsExpediente_Expedientes_ExpedienteID",
                table: "DocsExpediente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocsExpediente",
                table: "DocsExpediente");

            migrationBuilder.RenameTable(
                name: "DocsExpediente",
                newName: "DocExpedientes");

            migrationBuilder.RenameIndex(
                name: "IX_DocsExpediente_ExpedienteID",
                table: "DocExpedientes",
                newName: "IX_DocExpedientes_ExpedienteID");

            migrationBuilder.AddColumn<string>(
                name: "Base64",
                table: "DocExpedientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocExpedientes",
                table: "DocExpedientes",
                column: "DocID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocExpedientes_Expedientes_ExpedienteID",
                table: "DocExpedientes",
                column: "ExpedienteID",
                principalTable: "Expedientes",
                principalColumn: "ExpedienteID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
