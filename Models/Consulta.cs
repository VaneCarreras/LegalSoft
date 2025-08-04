using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
   public class Consulta
    {
        [Key]
        public int ConsultaID { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido.")]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "El equipo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un equipo válido.")]
        public int EquipoID { get; set; }

        public string? ClienteNombre { get; set; }
        public string? EquipoNombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 50 caracteres.")]
        public string? Motivo { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "El estado de la consulta es obligatorio.")]
        [EnumDataType(typeof(EstadoConsulta), ErrorMessage = "Estado de consulta inválido.")]
        public EstadoConsulta EstadoConsulta { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual Equipo? Equipo { get; set; }

        public bool Habilitado { get; set; }
    }

   public enum EstadoConsulta
   {
      Asesorada = 1,
      Judicializada,
      Desestimada,
   Iniciada
}
   public class VistaConsulta
   {
      public int ConsultaID { get; set; }
      public int ClienteID { get; set; }
      public int EquipoID { get; set; }
      public string NombreCompletoCliente { get; set; }
      public string NombreCompletoEquipo { get; set; }

      public string? Descripcion { get; set; }
      public string? Motivo { get; set; }

      public DateOnly Fecha { get; set; }
      public virtual Cliente? Cliente { get; set; }
      public virtual Equipo? Equipo { get; set; }
      public EstadoConsulta EstadoConsulta { get; set; }
      public string? EstadoConsultaString { get; set; }
      public bool Habilitado { get; set; }

   }
}