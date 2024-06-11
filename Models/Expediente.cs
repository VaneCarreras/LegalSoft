using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Expediente
    {
       [Key]
       public int ExpedienteID { get; set;} 
       public string Numero { get; set;}
       public string Caratula { get; set;}
       public string UltimoDecreto { get; set;}
       public DateTime FechaInicio { get; set;}
       public DateTime FechaFin { get; set;}
       public string LinkContenido { get; set;}

       [ForeignKey("Cliente")]
       public int ClienteID { get; set; }
       public Cliente Cliente { get; set; }

       [ForeignKey("Equipo")]
       public int EquipoID { get; set; }
       public Equipo Equipo { get; set; }
    }
}