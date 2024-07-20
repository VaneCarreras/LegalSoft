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
       public int PersonaID { get; set;} 
       public string? NroLegajo { get; set;}
       public virtual ICollection<Turno>? Turnos { get; set; }
       public virtual ICollection<Consulta>? Consultas { get; set; }
       public virtual ICollection<Expediente>? Expedientes { get; set; }
       public virtual ICollection<Pendiente>? Pendientes { get; set; }



    }

    public class VistaEquipo
    {
       public int EquipoID { get; set;} 
       public string? NroLegajo { get; set;}
       public string? NombreCompleto { get; set;}
       public string? NroTipoDoc { get; set;}
       public string? Direccion { get; set;}
       public string? Telefono { get; set;}
       public DateTime FechaNac { get; set;}
    }
}