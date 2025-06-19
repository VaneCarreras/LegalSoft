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
       public int ClienteID { get; set; }
       public int EquipoID { get; set; }
       public string? ClienteNombre { get; set;}
       public string? EquipoNombre {get; set;}
       public string? Descripcion { get; set; }
       public DateOnly Fecha { get; set;}
       public virtual Cliente ?Cliente { get; set; }
       public virtual Equipo ?Equipo { get; set; }
    }

    public class VistaConsulta
    {
       public int ConsultaID { get; set;} 
       public int ClienteID { get; set; }
       public int EquipoID { get; set; }
       public string NombreCompletoCliente { get; set; }
       public string NombreCompletoEquipo { get; set; }
       
       public string? Descripcion { get; set;}
       public DateOnly Fecha { get; set;}
       public virtual Cliente ?Cliente { get; set; }
       public virtual Equipo ?Equipo { get; set; }
    }
}