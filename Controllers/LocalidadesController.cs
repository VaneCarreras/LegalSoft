using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LegalSoft.Controllers;

[Authorize]

public class LocalidadesController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    //CONSTRUCTOR
    public LocalidadesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
  public IActionResult Index()
{
    

    ViewBag.Provincia = new SelectList(
    Enum.GetValues(typeof(Provincia))
        .Cast<Provincia>()
        .Select(e => new { Value = (int)e, Text = e.ToString().ToUpper() }),
    "Value",
    "Text"
);



    return View();
}

public JsonResult ListadoLocalidades(int? id)
{
    // Obtener la lista de consultas
    var localidades = _context.Localidades.ToList();

    // Filtrar por ID si es proporcionado
    if (id.HasValue)
    {
        localidades = localidades.Where(c => c.LocalidadID == id.Value).ToList();
    }

    // Crear una lista de consultas para mostrar
    List<VistaLocalidad> localidadesMostrar = new List<VistaLocalidad>();

    foreach (var localidad in localidades)
    {
        

        var localidadMostrar = new VistaLocalidad
        {
            LocalidadID = localidad.LocalidadID,
            
            LocalidadNombre = localidad.LocalidadNombre,
            
            Provincia = localidad.Provincia,
            ProvinciaString = localidad.Provincia.ToString().ToUpper(),    // <-- Este campo ahora existe
        };

        localidadesMostrar.Add(localidadMostrar);
    }

    return Json(localidadesMostrar);
}


    public JsonResult GuardarNuevaLocalidad(int localidadID, string? localidadNombre, Provincia provincia)
    {

        var error = 0;

         if (error == 0)

        {

            //4- GUARDAR
            var localidad = new Localidad
            {
                LocalidadID = localidadID,
                LocalidadNombre = localidadNombre,
                Provincia = provincia,
            };
            _context.Add(localidad);
            _context.SaveChanges();

        }
            
            else
            {
                // //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                // var consultaEditar = _context.Consultas.Where(c => c.ConsultaID == consultaID).SingleOrDefault();
                // if (consultaEditar != null)
                // {
                //     consultaEditar.ConsultaID = consultaID;
                //     consultaEditar.ClienteID = clienteID;
                //     consultaEditar.EquipoID = equipoID;
                //     consultaEditar.Descripcion = descripcion;
                //     consultaEditar.Fecha = fecha;
                //     _context.SaveChanges();
                // }
            }
        

        return Json(error);
    }

public JsonResult EditarLocalidad(int localidadID,  string? localidadNombre,  Provincia provincia)
{
    // Buscar el cliente por el ID proporcionado
    var localidadEditar = _context.Localidades.SingleOrDefault(c => c.LocalidadID == localidadID);

    // Si el cliente existe, buscamos la persona relacionada
    if (localidadEditar != null)
    {
        
        // Si la persona existe, actualizamos sus datos
        if (localidadEditar != null)
        {
            localidadEditar.LocalidadNombre = localidadNombre;
            localidadEditar.Provincia = provincia;

            // Guardamos los cambios en la base de datos
                _context.SaveChanges();

            return Json("localidad actualizada correctamente");
        }
        else
        {
            return Json("Error");
        }
    }
    else
    {
        return Json("Error: localidad no encontrado");
    }
}

    public JsonResult EliminarLocalidad(int localidadID)
    {
        var localidad = _context.Localidades.Find(localidadID);
        _context.Remove(localidad);
        _context.SaveChanges();

        return Json(true);
    }

    
}