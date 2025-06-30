using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class DocsExpediente
    {
        [Key]
        public int DocID { get; set; }
        public int ExpedienteID { get; set; }
        public byte[]? Imagen { get; set; }
        public string? TipoImg { get; set; }
        public string? NombreArchivo { get; set; }
        public string? Descripcion { get; set; }
        public virtual Expediente? Expediente { get; set; }


    }

    public class VistaDocsExpediente
    {
        public int DocID { get; set; }
        public string NombreArchivo { set; get; }
        public string Base64 { get; set; }
    }
}