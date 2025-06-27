using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Expediente
    {
        [Key]
        public int ExpedienteID { get; set; }
        public int ClienteID { get; set; }
        public int EquipoID { get; set; }
        public string? Numero { get; set; }
        public string? Caratula { get; set; }
        public string? UltimoDecreto { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string? LinkContenido { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public virtual Equipo? Equipo { get; set; }
        public virtual ICollection<DocLegal>? DocLegales { get; set; }
        public EstadoExpediente EstadoExpediente { get; set; }


    }
    
       public enum EstadoExpediente
   {
      En_Curso= 1,
      Con_Sentencia
   }

    public class VistaExpediente
    {
        public int ExpedienteID { get; set; }
        public int ClienteID { get; set; }
        public int EquipoID { get; set; }
        public string NombreCompletoCliente { get; set; }
        public string NombreCompletoEquipo { get; set; }
        public string? Numero { get; set; }
        public string? Caratula { get; set; }
        public string? UltimoDecreto { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string? LinkContenido { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public virtual Equipo? Equipo { get; set; }
        public virtual ICollection<DocLegal>? DocLegales { get; set; }
        public EstadoExpediente EstadoExpediente { get; set; }
        public string? EstadoExpedienteString { get; set; }
    }
}