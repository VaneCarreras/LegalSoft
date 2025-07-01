using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Turno
    {
        [Key]
        public int TurnoID { get; set; }
        public int ClienteID { get; set; }
        public int EquipoID { get; set; }
        public DateTime FechaHora { get; set; }
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