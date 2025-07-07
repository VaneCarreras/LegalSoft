using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using LegalSoft.Models;

namespace LegalSoft.Models
{
    public class Localidad
    {
        [Key]
        public int LocalidadID { get; set; }
        public string? LocalidadNombre { get; set; }
        public Provincia Provincia { get; set; }

    }

    public enum Provincia
    {
        Buenos_Aires,
        Catamarca,
        Chaco,
        Chubut,
        Cordoba,
        Entre_Rios,
        Corrientes,
        Mendoza,
        Jujuy,
        Formosa,
        La_Pampa,
        Misiones,
        Neuquen,
        Rio_Negro,
        Salta,
        San_Juan,

        San_Luis,
        Santa_Cruz,
        Santa_Fe,
        Santiago_Del_Estero,
        Tierra_Del_Fuego,
        Tucuman,
        La_Rioja

    }

    public class VistaLocalidad
    {
        public int LocalidadID { get; set; }
        public string? LocalidadNombre { get; set; }
        public Provincia Provincia { get; set; }
        public string? ProvinciaString { get; set; }
}


}