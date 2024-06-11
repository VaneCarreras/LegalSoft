using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Turno
    {
       [Key]
       public int TurnoID { get; set;} 
       public DateTime FechaHora { get; set;}
       
       [ForeignKey("Cliente")]
       public int ClienteID { get; set; }
       public Cliente Cliente { get; set; }

       [ForeignKey("Equipo")]
       public int EquipoID { get; set; }
       public Equipo Equipo { get; set; }
    }
}