using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class DocLegal
    {
       [Key]
       public int DocLegalID { get; set;} 
       public int ExpedienteID { get; set;}
       public byte[]? Imagen { get; set;}
       public string? TipoImg { get; set;}
       public string? NombreArchivo { get; set;}
       public string? Descripcion { get; set;}
       public virtual Expediente ?Expediente { get; set; }


    } 
}