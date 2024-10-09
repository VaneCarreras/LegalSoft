using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
// using Microsoft.AspNetCore.Authorization;
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
    // Obtener la lista de clientes
    var equipos = _context.Equipos.ToList();

    // Filtrar por ID si es proporcionado
    if (id.HasValue)
    {
        equipos = equipos.Where(e => e.EquipoID == id.Value).ToList();
    }

    // Crear una lista de clientes para mostrar, accediendo a la entidad Persona por PersonaID
    List<VistaEquipo> equiposMostrar = new List<VistaEquipo>();
    foreach (var equipo in equipos)
    {
        // Obtener la persona relacionada a través del PersonaID
        var persona = _context.Personas.SingleOrDefault(p => p.PersonaID == equipo.PersonaID);

        if (persona != null)
        {
            var equipoMostrar = new VistaEquipo
            {
                EquipoID = equipo.EquipoID,
                NombreCompleto = persona.NombreCompleto,
                NroTipoDoc = persona.NroTipoDoc,
                Direccion = persona.Direccion,
                Telefono = persona.Telefono,
                FechaNac = persona.FechaNac
            };

            equiposMostrar.Add(equipoMostrar);
        }
    }

    // Ordenar por nombre completo antes de devolver
    return Json(equiposMostrar.OrderBy(e => e.NombreCompleto).ToList());
}


    public JsonResult BuscarEquipos(string nombreCompleto, string nroTipoDoc)
    {
        var personas = _context.Personas.ToList();

        if (!string.IsNullOrEmpty(nombreCompleto))
        {
            personas = personas.Where(e => e.NombreCompleto.ToLower().Contains(nombreCompleto.ToLower())).ToList();
        }

        if (!string.IsNullOrEmpty(nroTipoDoc))
        {
            personas = personas.Where(e => e.NroTipoDoc.Contains(nroTipoDoc)).ToList();
        }

        List<VistaEquipo> equiposMostrar = new List<VistaEquipo>();
        foreach (var persona in personas.OrderBy(e => e.NombreCompleto))
        {
            var equipo = _context.Equipos.Where(e => e.PersonaID == persona.PersonaID).SingleOrDefault();
            if (equipo != null)
            {
                var equipoMostrar = new VistaEquipo
                {
                    EquipoID = equipo.EquipoID,
                    NombreCompleto = persona.NombreCompleto,
                    NroTipoDoc = persona.NroTipoDoc,
                    Direccion = persona.Direccion,
                    Telefono = persona.Telefono,
                    FechaNac = persona.FechaNac
                };
                equiposMostrar.Add(equipoMostrar);
            }
        }

        return Json(equiposMostrar);
    }


    public JsonResult GuardarNuevoEquipo(string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac)
    {
        int error = 0;
        string resultado = "";
        string usuarioID = "";

        if (error == 0)
        {


            var persona = new Persona
            {
                NombreCompleto = nombreCompleto,
                UsuarioID = usuarioID,
                NroTipoDoc = nroTipoDoc,
                Direccion = direccion,
                Telefono = telefono,
                FechaNac = fechaNac,
            };
            _context.Add(persona);
            _context.SaveChanges();

            var equipo = new Equipo
            {
                PersonaID = persona.PersonaID,
            };
            _context.Add(equipo);
            _context.SaveChanges();




        }

        return Json(resultado);
    }

    public JsonResult EliminarEquipo(int EquipoID)
    {
        var equipo = _context.Equipos.Find(EquipoID);
        _context.Remove(equipo);
        _context.SaveChanges();

        return Json(true);
    }

[HttpPost]
public JsonResult EditarEquipo(int EquipoID, string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac)
{
    // Buscar el cliente por el ID proporcionado
    var equipoEditar = _context.Equipos.SingleOrDefault(e => e.EquipoID == EquipoID);

    // Si el cliente existe, buscamos la persona relacionada
    if (equipoEditar != null)
    {
        var personaEditar = _context.Personas.SingleOrDefault(p => p.PersonaID == equipoEditar.PersonaID);
        
        // Si la persona existe, actualizamos sus datos
        if (personaEditar != null)
        {
            personaEditar.NroTipoDoc = nroTipoDoc;
            personaEditar.NombreCompleto = nombreCompleto;
            personaEditar.Direccion = direccion;
            personaEditar.Telefono = telefono;
            personaEditar.FechaNac = fechaNac;

            // Guardamos los cambios en la base de datos
            _context.SaveChanges();

            return Json("Cliente actualizado correctamente");
        }
        else
        {
            return Json("Error: Persona asociada no encontrada");
        }
    }
    else
    {
        return Json("Error: Cliente no encontrado");
    }
}



}



