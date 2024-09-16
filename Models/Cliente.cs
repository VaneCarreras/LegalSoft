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

    public class VistaCliente
    {
       public int ClienteID { get; set;} 
       public string? NombreCompleto { get; set;}
       public string? NroTipoDoc { get; set;}
       public string? Direccion { get; set;}
       public string? Telefono { get; set;}
       public DateOnly FechaNac { get; set;}
    }

}