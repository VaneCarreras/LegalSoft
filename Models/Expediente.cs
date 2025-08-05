using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{public class Expediente
{
    [Key]
    public int ExpedienteID { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un cliente.")]
    [Range(1, int.MaxValue, ErrorMessage = "Cliente inválido.")]
    public int ClienteID { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un equipo.")]
    [Range(1, int.MaxValue, ErrorMessage = "Equipo inválido.")]
    public int EquipoID { get; set; }

    [StringLength(50, ErrorMessage = "El número no puede tener más de 50 caracteres.")]
    public string? Numero { get; set; }

    [Required(ErrorMessage = "La carátula es obligatoria.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "La carátula debe tener entre 3 y 200 caracteres.")]
    public string? Caratula { get; set; }

    [Required(ErrorMessage = "El último decreto es obligatorio.")]
    [StringLength(500, MinimumLength = 3, ErrorMessage = "El último decreto debe tener entre 3 y 500 caracteres.")]
    public string? UltimoDecreto { get; set; }

    [Required(ErrorMessage = "Debe ingresar una fecha de inicio.")]
    public DateOnly FechaInicio { get; set; }

    [Url(ErrorMessage = "El link de contenido debe ser una URL válida.")]
    [StringLength(300, ErrorMessage = "El link no puede tener más de 300 caracteres.")]
    public string? LinkContenido { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un área.")]
    public Area Area { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una dependencia.")]
    public Dependencia Dependencia { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una ubicación.")]
    public Ubicacion Ubicacion { get; set; }


    public EstadoExpediente EstadoExpediente { get; set; }

    public virtual Cliente? Cliente { get; set; }
    public virtual Equipo? Equipo { get; set; }
    public virtual ICollection<DocsExpediente>? DocExpedientes { get; set; }

    public bool Habilitado { get; set; }
}
    
       public enum EstadoExpediente
   {
      En_Curso= 1,
      Con_Sentencia
   }

    public enum Area
    {
        Civil = 1,
        Penal,
        Laboral,
        Familiar,
      Comercial
   }
    public enum Dependencia
    {
        Morteros = 1,
        San_Francisco,
        Cordoba,
      Santa_Fe
   }
    public enum Ubicacion
    {
        Casillero = 1,
        Despacho,
        En_Letra,
        Reservado,
      Archivado
   }

    public class VistaExpediente
    {
        public int ExpedienteID { get; set; }
        public int ClienteID { get; set; }
        public int EquipoID { get; set; }
        public string NombreCompletoCliente { get; set; }
        public string NombreCompletoEquipo { get; set; }
        public string? Numero { get; set; }
        public string? Caratula { get; set; }
        public string? UltimoDecreto { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string? LinkContenido { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public virtual Equipo? Equipo { get; set; }
        public virtual ICollection<DocsExpediente>? DocExpedientes { get; set; }
        public EstadoExpediente EstadoExpediente { get; set; }
        public string? EstadoExpedienteString { get; set; }
        public Area Area { get; set; }
        public string? AreaString { get; set; }
        public Dependencia Dependencia { get; set; }
        public string? DependenciaString { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public string? UbicacionString { get; set; }
        public bool Habilitado { get; set; }

    }
}