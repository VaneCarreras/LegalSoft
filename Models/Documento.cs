using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Documento
    {
       [Key]
       public int DocumentoID { get; set;} 
       public byte Imagen { get; set;}

       [ForeignKey("Persona")]
       public int PersonaID { get; set; }
       public Persona Persona { get; set; }
    } 
}