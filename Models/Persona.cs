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
       public string? UsuarioID { get; set;}
       public string? NroTipoDoc { get; set;}
       public string? NombreCompleto { get; set;}
       public string? Direccion { get; set;}
       public string? Telefono { get; set;}
       public DateOnly FechaNac { get; set;}
       //public virtual ICollection<Cliente>? Clientes { get; set; }



    }

    
    public class VistaPersona
    {
       public int PersonaID { get; set;}
       public string? UsuarioID {get; set;} 
       public string? NombreCompleto { get; set;}
       public string? NroTipoDoc { get; set;}
       public string? Direccion { get; set;}
       public string? Telefono { get; set;}
       public DateOnly FechaNac { get; set;}
       public string? FechaString { get; set;}

    }
}