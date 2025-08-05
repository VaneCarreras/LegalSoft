using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LegalSoft.Controllers;

// [Authorize]
    [Authorize(Roles = "Administrador")]

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

    [HttpGet]
    public JsonResult ObtenerLocalidades()
    {
        var localidades = _context.Localidades
            .Select(l => new
            {
                localidadID = l.LocalidadID,
                localidadNombre = l.LocalidadNombre
            }).ToList();

        return Json(localidades);
    }
    [HttpPost]

    public JsonResult ListadoEquipos(int? id, bool soloHabilitados = false)
    {
        // Obtener la lista de clientes
        var equipos = _context.Equipos.ToList();

        // Filtrar por ID si es proporcionado
        if (id.HasValue)
        {
            equipos = equipos.Where(e => e.EquipoID == id.Value).ToList();
        }

        if (soloHabilitados)
        {
            equipos = equipos.Where(e => e.Habilitado).ToList();
        }

        // Crear una lista de clientes para mostrar, accediendo a la entidad Persona por PersonaID
        List<VistaEquipo> equiposMostrar = new List<VistaEquipo>();
        foreach (var equipo in equipos)
        {
            // Obtener la persona relacionada a través del PersonaID
            var persona = _context.Personas.Include(p => p.Localidad).SingleOrDefault(p => p.PersonaID == equipo.PersonaID);

            if (persona != null)
            {
                var equipoMostrar = new VistaEquipo
                {
                    EquipoID = equipo.EquipoID,
                    NombreCompleto = persona.NombreCompleto,
                    NroTipoDoc = persona.NroTipoDoc,
                    Direccion = persona.Direccion,
                    Telefono = persona.Telefono,
                    FechaNac = persona.FechaNac,
                    LocalidadNombre = persona.Localidad?.LocalidadNombre,
                    Habilitado = equipo.Habilitado,

                };

                equiposMostrar.Add(equipoMostrar);
            }
        }

        // Ordenar por nombre completo antes de devolver
        return Json(equiposMostrar.OrderBy(e => e.NombreCompleto).ToList());
    }

    [HttpPost]

    public JsonResult BuscarEquipos(string nombreCompleto, string nroTipoDoc, bool soloHabilitados = false)
    {
        var personas = _context.Personas.Include(p => p.Localidad).ToList();


        if (soloHabilitados)
        {
            personas = personas
                .Where(p => _context.Equipos.Any(e => e.PersonaID == p.PersonaID && e.Habilitado))
                .ToList();
        }


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
                    FechaNac = persona.FechaNac,
                    LocalidadNombre = persona.Localidad?.LocalidadNombre,
                    Habilitado = equipo.Habilitado,

                };
                equiposMostrar.Add(equipoMostrar);
            }
        }

        return Json(equiposMostrar);
    }

    [HttpPost]

    public JsonResult GuardarNuevoEquipo(string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac, int localidadID, string localidadNombre)
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
                LocalidadID = localidadID,

            };
            _context.Add(persona);
            _context.SaveChanges();

            var equipo = new Equipo
            {
                PersonaID = persona.PersonaID,
                Habilitado = true,

            };
            _context.Add(equipo);
            _context.SaveChanges();




        }

        return Json(resultado);
    }
    [HttpPost]

    public JsonResult EliminarEquipo(int EquipoID)
    {
        var equipo = _context.Equipos.Find(EquipoID);
        _context.Remove(equipo);
        _context.SaveChanges();

        return Json(true);
    }

    [HttpPost]
    public JsonResult CambiarEstadoEquipo(int EquipoID, bool habilitar)
    {
        try
        {
            var equipo = _context.Equipos.Find(EquipoID);
            if (equipo != null)
            {
                equipo.Habilitado = habilitar;
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Equipo no encontrado." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public JsonResult EditarEquipo(int EquipoID, string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac, int localidadID)
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
                personaEditar.LocalidadID = localidadID;


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



