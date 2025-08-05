using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    
    public class FutureOrPresentDatePAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            return date >= DateTime.Now;
        }
        return false;
    }
}


public class Pendiente
    {
        [Key]
        public int PendienteID { get; set; }

        [Required]
        public int EquipoID { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El motivo debe tener entre 3 y 50 caracteres.")]
        public string? Motivo { get; set; }

        public bool RecordatorioAlert { get; set; }

        [Required]
        public string Estado { get; set; } = "No realizado";

        public virtual Equipo? Equipo { get; set; }
    }

}