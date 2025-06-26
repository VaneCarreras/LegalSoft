using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class ImagenCliente
    {
        [Key]
        public int ImagenClienteID { get; set; }
        public int ClienteID { get; set; }
        public byte[]? Imagen { get; set; }
        // public string? TipoImg { get; set; }
        // public string? NombreArchivo { get; set; }
        public virtual Cliente Cliente { get; set; }

    } 
    

    public class VistaImagenCliente
    {
        public int ImgClientesID { get; set; }
        public string? Base64 { get; set; }
    }
}