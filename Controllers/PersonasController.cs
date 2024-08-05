using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LegalSoft.Controllers;

// [Authorize]
public class PersonasController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public PersonasController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoPersonas(int? id)
    {

        var personas = _context.Personas.ToList();
        if (id != null)
        {
            personas = personas.Where(p => p.PersonaID == id).ToList();
        }

        var personasMostrar = personas
        .Select(p => new VistaPersona
        //
        {
            PersonaID = p.PersonaID,
            UsuarioID = p.UsuarioID,
            NombreCompleto = p.NombreCompleto,
            NroTipoDoc = p.NroTipoDoc,
            Direccion = p.Direccion,
            Telefono = p.Telefono,
            FechaNac = p.FechaNac,
            FechaString = p.FechaNac.ToString("dd/MM/yyyy"),
        })
        .OrderBy(p => p.NombreCompleto).ToList();



        return Json(personasMostrar);
    }

    public JsonResult GuardarPersona(int personaID, string usuarioID, string nombreCompleto, string nroTipoDoc, string direccion, string telefono, DateOnly fechaNac)
    {
        int error = 0;

        if(error == 0)
        {        
            if (personaID == 0)
            {
                //4- GUARDAR LA PERSONA ------------
                var persona = new Persona
                {
                    NombreCompleto = nombreCompleto,
                    NroTipoDoc = nroTipoDoc,
                    Direccion = direccion,
                    Telefono = telefono,
                    FechaNac = fechaNac,
                };
                _context.Add(persona);
                _context.SaveChanges();
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO-- 

                var personaEditar = _context.Personas.Where(p => p.PersonaID == personaID).SingleOrDefault();
                if (personaEditar != null)
                {
                    personaEditar.NombreCompleto = nombreCompleto;
                    personaEditar.UsuarioID = usuarioID;
                    personaEditar.NroTipoDoc = nroTipoDoc;
                    personaEditar.NombreCompleto = nombreCompleto;
                    personaEditar.Direccion = direccion;
                    personaEditar.Telefono = telefono;
                    personaEditar.FechaNac  = fechaNac;
                    _context.SaveChanges();
                }


                
            }
        }

        return Json(error);
    }

    public JsonResult EliminarPersona(int personaID)
    {
        var persona = _context.Personas.Find(personaID);
        _context.Remove(persona);
        _context.SaveChanges();

        return Json(true);
    }

}
