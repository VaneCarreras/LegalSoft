﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class personaCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Clientes_PersonaID",
                table: "Clientes",
                column: "PersonaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Personas_PersonaID",
                table: "Clientes",
                column: "PersonaID",
                principalTable: "Personas",
                principalColumn: "PersonaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Personas_PersonaID",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_PersonaID",
                table: "Clientes");
        }
    }
}