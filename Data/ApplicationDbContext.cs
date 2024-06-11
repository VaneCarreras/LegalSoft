using LegalSoft.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegalSoft.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<DocLegal> DocLegales { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<Expediente> Expedientes { get; set; }
    public DbSet<Pendiente> Pendientes { get; set; }
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Turno> Turnos { get; set; }

}
