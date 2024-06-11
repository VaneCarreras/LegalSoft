using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Consulta
    {
       [Key]
       public int ConsultaID { get; set;} 
       public string Descripcion { get; set;}
       public DateTime Fecha { get; set;}
       
       [ForeignKey("Cliente")]
       public int ClienteID { get; set; }
       public Cliente Cliente { get; set; }

       [ForeignKey("Equipo")]
       public int EquipoID { get; set; }
       public Equipo Equipo { get; set; }
    }
}