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
       public byte Imagen { get; set;}
       public string Descripcion { get; set;}

       [ForeignKey("Expediente")]
       public int ExpedienteID { get; set; }
       public Expediente Expediente { get; set; }
    } 
}