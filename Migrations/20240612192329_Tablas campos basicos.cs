using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegalSoft.Migrations
{
    /// <inheritdoc />
    public partial class Tablascamposbasicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    DocumentoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaID = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TipoImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.DocumentoID);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaID = table.Column<int>(type: "int", nullable: false),
                    NroLegajo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.EquipoID);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    PersonaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NroTipoDoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNac = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.PersonaID);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    ConsultaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.ConsultaID);
                    table.ForeignKey(
                        name: "FK_Consultas_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultas_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expedientes",
                columns: table => new
                {
                    ExpedienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caratula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UltimoDecreto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkContenido = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expedientes", x => x.ExpedienteID);
                    table.ForeignKey(
                        name: "FK_Expedientes_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expedientes_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pendientes",
                columns: table => new
                {
                    PendienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordatorioAlert = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pendientes", x => x.PendienteID);
                    table.ForeignKey(
                        name: "FK_Pendientes_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    TurnoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.TurnoID);
                    table.ForeignKey(
                        name: "FK_Turnos_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turnos_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocLegales",
                columns: table => new
                {
                    DocLegalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpedienteID = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TipoImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocLegales", x => x.DocLegalID);
                    table.ForeignKey(
                        name: "FK_DocLegales_Expedientes_ExpedienteID",
                        column: x => x.ExpedienteID,
                        principalTable: "Expedientes",
                        principalColumn: "ExpedienteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_ClienteID",
                table: "Consultas",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_EquipoID",
                table: "Consultas",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_DocLegales_ExpedienteID",
                table: "DocLegales",
                column: "ExpedienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Expedientes_ClienteID",
                table: "Expedientes",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Expedientes_EquipoID",
                table: "Expedientes",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_Pendientes_EquipoID",
                table: "Pendientes",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_ClienteID",
                table: "Turnos",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_EquipoID",
                table: "Turnos",
                column: "EquipoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "DocLegales");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Pendientes");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Expedientes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Equipos");
        }
    }
}
