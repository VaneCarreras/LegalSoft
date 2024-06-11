using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Equipo
    {
       [Key]
       public int EquipoID { get; set;} 
       public string NroLegajo { get; set;}
    }
}