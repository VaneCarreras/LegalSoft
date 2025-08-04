using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class FutureOrPresentDateTAttribute : ValidationAttribute
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
    public class Turno
    {
        [Key]
        public int TurnoID { get; set; }
                [Required]

        public int ClienteID { get; set; }
                [Required]

        public int EquipoID { get; set; }
        [Required]
        [FutureOrPresentDateT(ErrorMessage = "La fecha debe ser actual o futura.")]
        public DateTime FechaHora { get; set; }
        [Required]
        public EstadoTurno Estado { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual Equipo? Equipo { get; set; }
    }

    public enum EstadoTurno
    {
        Vacante,
        Asistido,
        Suspendido
    }



}