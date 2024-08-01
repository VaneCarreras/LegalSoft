using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LegalSoft.Controllers;

// [Authorize]
public class EquiposController : Controller
{

    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public EquiposController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoEquipos(int? id)
    {

        var equipos = _context.Equipos.ToList();


        if (id != null)
        {
            equipos = equipos.Where(e => e.EquipoID == id).ToList();

        }
/////////////////////////////////////////////////////////////////////////////////////////////////
        var equiposMostrar = equipos
        .Select(e => new VistaEquipo
        {
        })
        .ToList();


        return Json(equiposMostrar);
    }

    public JsonResult GuardarEquipo(int equipoID, int personaID, string nroLegajo, string usuarioID, string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac)
    {
        int error = 0;

        if(error == 0)
        {        
            if (equipoID == 0)
            {   //TIENE QUE GUARDAR Y EDITAR EN PERSONA Y EN EQUIPO A LA VEZ
                //4- GUARDAR EL EJERCICIO
                var equipo = new Equipo
                {
                    EquipoID = equipoID,
                };
                _context.Add(equipo);
                _context.SaveChanges();

                var persona = new Persona
                {
                    PersonaID = personaID,
                    NombreCompleto = nombreCompleto,
                    UsuarioID = usuarioID,
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
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var equipoEditar = _context.Equipos.Where(e => e.EquipoID == equipoID).SingleOrDefault();
                if (equipoEditar != null)
                {
                    equipoEditar.PersonaID = personaID; 
                    equipoEditar.NroLegajo = nroLegajo;
                    _context.SaveChanges();
                }

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

    public JsonResult EliminarEquipo(int equipoID)
    {
        var equipo = _context.Equipos.Find(equipoID);
        _context.Remove(equipo);
        _context.SaveChanges();

        return Json(true);
    }

}