using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegalSoft.Models;
using Microsoft.AspNetCore.Identity;

namespace LegalSoft.Models
{
    public class Persona
    {
       [Key]
       public int PersonaID { get; set;} 
       public string DNI { get; set;}
       public string Nombre { get; set;}
       public string Direccion { get; set;}
       public string Telefono { get; set;}
       public DateTime FechaNac { get; set;}
       public string Cuil_cuit { get; set;}

    }
}