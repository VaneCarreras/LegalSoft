using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegalSoft.Models;
using Microsoft.AspNetCore.Identity;

namespace LegalSoft.Models
{
   public class Persona
   {
    [Key]
    public int PersonaID { get; set; }

    public string? UsuarioID { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una localidad.")]
    public int? LocalidadID { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 dígitos.")]
    public string? NroTipoDoc { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 letras.")]
    [RegularExpression(@"^[A-ZÁÉÍÓÚÑ\s]+$", ErrorMessage = "Solo letras mayúsculas y espacios.")]
    public string? NombreCompleto { get; set; }

    [StringLength(100)]
    public string? Direccion { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^[1-9]\d{6,9}$", ErrorMessage = "Teléfono sin 0 ni 15, solo números (mínimo 7 dígitos).")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "Debe ingresar una fecha de nacimiento.")]
    public DateOnly FechaNac { get; set; }

    public virtual Localidad? Localidad { get; set; }
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