using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Pendiente
    {
       [Key]
       public int PendienteID { get; set;} 
       public int EquipoID { get; set; }
       public DateTime FechaHora { get; set;}
       public string? Motivo { get; set;}
       public bool RecordatorioAlert { get; set;}
           public string Estado { get; set; } = "No realizado"; 

       public virtual Equipo? Equipo { get; set; }

    }
}