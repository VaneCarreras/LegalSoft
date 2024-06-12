using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Cliente
    {
       [Key]
       public int ClienteID { get; set;} 
       public int PersonaID { get; set;} 
       public virtual ICollection<Turno>? Turnos { get; set; }
       public virtual ICollection<Consulta>? Consultas { get; set; }
       public virtual ICollection<Expediente>? Expedientes { get; set; }



    }

}