using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LegalSoft.Models
{
    public class Contacto
    {

        public int ContactoID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        public string Email { get; set; }



        public string? AreaSeleccionada { get; set; }  // Para saber qué área eligió

        // Campos opcionales según el área
        public string? Empleador { get; set; }
        public string? Horas { get; set; }
        public string? Situacion { get; set; }
        public string? Sueldo { get; set; }

        public string? Vinculo { get; set; }
        public string? Datos { get; set; }
        public string? Datos2 { get; set; }

        public string? Motivo { get; set; }
        public string? Detalle { get; set; }
        public string? Delito { get; set; }
    public string? Tipo { get; set; }



    }
}