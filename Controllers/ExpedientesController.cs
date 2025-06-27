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

public class ExpedientesController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    //CONSTRUCTOR
    public ExpedientesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
  public IActionResult Index()
{
    // Obtener personas del contexto correspondiente (el otro contexto si es necesario)
    var personas = _context.Personas
        .Select(p => new { p.PersonaID, p.NombreCompleto })
        .ToList();

    // Obtener clientes del contexto actual
    var clientes = _context.Clientes
        .Select(c => new { c.ClienteID, c.PersonaID })
        .ToList();

    // Unir ambas listas manualmente por PersonaID
    var clientesConNombre = (from c in clientes
                             join p in personas on c.PersonaID equals p.PersonaID
                             select new
                             {
                                 ClienteID = c.ClienteID,
                                 NombreCompleto = p.NombreCompleto
                             }).ToList();

    var clientesBuscar = clientesConNombre.ToList();

    clientesConNombre.Add(new { ClienteID = 0, NombreCompleto = "[SELECCIONE...]" });
    ViewBag.ClienteID = new SelectList(clientesConNombre.OrderBy(c => c.NombreCompleto), "ClienteID", "NombreCompleto");

    clientesBuscar.Add(new { ClienteID = 0, NombreCompleto = "[TODOS]" });
    ViewBag.NombreCompletoClienteBuscar = new SelectList(clientesBuscar.OrderBy(c => c.NombreCompleto), "ClienteID", "NombreCompleto");


        var equipos = _context.Equipos
        .Select(e => new { e.EquipoID, e.PersonaID })
        .ToList();

    var equiposConNombre = (from e in equipos
                            join p in personas on e.PersonaID equals p.PersonaID
                            select new
                            {
                                EquipoID = e.EquipoID,
                                NombreCompleto = p.NombreCompleto
                            }).ToList();

    var equiposBuscar = equiposConNombre.ToList();

    equiposConNombre.Add(new { EquipoID = 0, NombreCompleto = "[SELECCIONE...]" });
    ViewBag.EquipoID = new SelectList(equiposConNombre.OrderBy(e => e.NombreCompleto), "EquipoID", "NombreCompleto");

    equiposBuscar.Add(new { EquipoID = 0, NombreCompleto = "[TODOS]" });
    ViewBag.NombreCompletoEquipoBuscar = new SelectList(equiposBuscar.OrderBy(e => e.NombreCompleto), "EquipoID", "NombreCompleto");

    ViewBag.EstadoExpediente = new SelectList(
    Enum.GetValues(typeof(EstadoExpediente))
        .Cast<EstadoExpediente>()
        .Select(e => new { Value = (int)e, Text = e.ToString().ToUpper() }),
    "Value",
    "Text"
);



    return View();
}

public JsonResult ListadoExpedientes(int? id)
{
    // Obtener la lista de consultas
    var expedientes = _context.Expedientes.ToList();

    // Filtrar por ID si es proporcionado
    if (id.HasValue)
    {
        expedientes = expedientes.Where(c => c.ExpedienteID == id.Value).ToList();
    }

    // Crear una lista de consultas para mostrar
    List<VistaExpediente> expedientesMostrar = new List<VistaExpediente>();

    foreach (var expediente in expedientes)
    {
        // Obtener el nombre completo del cliente
        var clienteNombre = _context.Clientes
            .Where(cli => cli.ClienteID == expediente.ClienteID)
            .Join(_context.Personas, cli => cli.PersonaID, p => p.PersonaID, (cli, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Cliente]";

        // Obtener el nombre completo del equipo
        var equipoNombre = _context.Equipos
            .Where(eq => eq.EquipoID == expediente.EquipoID)
            .Join(_context.Personas, eq => eq.PersonaID, p => p.PersonaID, (eq, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Equipo]";

        var expedienteMostrar = new VistaExpediente
        {
            ExpedienteID = expediente.ExpedienteID,
            ClienteID = expediente.ClienteID,
            EquipoID = expediente.EquipoID,
            Numero = expediente.Numero,
            Caratula = expediente.Caratula,
            // UltimoDecreto = expediente.UltimoDecreto,
            FechaInicio = expediente.FechaInicio,
            FechaFin = expediente.FechaFin,
            NombreCompletoCliente = clienteNombre, // <-- Este campo ahora existe
            NombreCompletoEquipo = equipoNombre,
            LinkContenido = expediente.LinkContenido,
            EstadoExpediente = expediente.EstadoExpediente,
            EstadoExpedienteString = expediente.EstadoExpediente.ToString().ToUpper(),    // <-- Este campo ahora existe
        };

        expedientesMostrar.Add(expedienteMostrar);
    }

    return Json(expedientesMostrar);
}




public JsonResult BuscarExpedientes(string DniClienteBuscar, string NroExpBuscar)
{
    // Obtener la lista de consultas
    var expedientes = _context.Expedientes.ToList();

    // Crear una lista de consultas para mostrar
    List<VistaExpediente> expedientesMostrar = new List<VistaExpediente>();

    foreach (var expediente in expedientes)
    {
        // Obtener el nombre completo del cliente
        var clienteNombre = _context.Clientes
            .Where(cli => cli.ClienteID == expediente.ClienteID)
            .Join(_context.Personas, cli => cli.PersonaID, p => p.PersonaID, (cli, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Cliente]";

        // Obtener el nombre completo del equipo
        var equipoNombre = _context.Equipos
            .Where(eq => eq.EquipoID == expediente.EquipoID)
            .Join(_context.Personas, eq => eq.PersonaID, p => p.PersonaID, (eq, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Equipo]";

        var expedienteMostrar = new VistaExpediente
        {
            ExpedienteID = expediente.ExpedienteID,
            Numero = expediente.Numero,
            Caratula = expediente.Caratula,
            FechaInicio = expediente.FechaInicio,
            FechaFin = expediente.FechaFin,
            ClienteID = expediente.ClienteID,
            EquipoID = expediente.EquipoID,
            // UltimoDecreto = expediente.UltimoDecreto,
            
            NombreCompletoCliente = clienteNombre,
            NombreCompletoEquipo = equipoNombre,
            LinkContenido = expediente.LinkContenido,
            EstadoExpediente = expediente.EstadoExpediente,
            EstadoExpedienteString = expediente.EstadoExpediente.ToString().ToUpper(),    // <-- Este campo ahora existe
        };

        expedientesMostrar.Add(expedienteMostrar);
    }

    // Ahora sí, aplicar el filtro sobre consultasMostrar, que **sí tiene** NombreCompletoCliente y NombreCompletoEquipo
    if (!string.IsNullOrEmpty(DniClienteBuscar))
    {
        expedientesMostrar = expedientesMostrar
            .Where(x => x.NombreCompletoCliente.ToLower().Contains(DniClienteBuscar.ToLower()))
            .ToList();
    }

    if (!string.IsNullOrEmpty(NroExpBuscar))
    {
        expedientesMostrar = expedientesMostrar
            .Where(x => x.Numero.ToLower().Contains(NroExpBuscar.ToLower()))
            .ToList();
    }

    return Json(expedientesMostrar);
}





    public JsonResult GuardarNuevoExpediente(int expedienteID, int clienteID, int equipoID, DateOnly fechaInicio, DateOnly fechaFin, string? nombreCompletoCliente, string? nombreCompletoEquipo, EstadoExpediente estadoExpediente, string? numero, string? caratula, string? ultimoDecreto, string? linkContenido)
    {

        var error = 0;

         if (error == 0)

        {

            //4- GUARDAR
            var expediente = new Expediente
            {
                ExpedienteID = expedienteID,
                ClienteID = clienteID,
                EquipoID = equipoID,
                Numero = numero,
                Caratula = caratula,
                // UltimoDecreto = ultimoDecreto,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                LinkContenido = linkContenido,
                EstadoExpediente = estadoExpediente,
            };
            _context.Add(expediente);
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

public JsonResult EditarExpediente(int expedienteID, int clienteID, int equipoID, DateOnly fechaInicio, DateOnly fechaFin, string? numero, string? caratula, string? ultimoDecreto, string? linkContenido, string? nombreCompletoCliente, string? nombreCompletoEquipo, EstadoExpediente estadoExpediente)
{
    // Buscar el cliente por el ID proporcionado
    var expedienteEditar = _context.Expedientes.SingleOrDefault(c => c.ExpedienteID == expedienteID);

    // Si el cliente existe, buscamos la persona relacionada
    if (expedienteEditar != null)
    {
        
        // Si la persona existe, actualizamos sus datos
        if (expedienteEditar != null)
        {
            expedienteEditar.ClienteID = clienteID;
            expedienteEditar.EquipoID = equipoID;
            expedienteEditar.Numero = numero;
            expedienteEditar.Caratula = caratula;
            expedienteEditar.LinkContenido = linkContenido;
            // expedienteEditar.UltimoDecreto = ultimoDecreto;
            expedienteEditar.FechaInicio = fechaInicio;
            expedienteEditar.FechaFin = fechaFin;
            expedienteEditar.EstadoExpediente = estadoExpediente;

            // Guardamos los cambios en la base de datos
                _context.SaveChanges();

            return Json("Expediente actualizada correctamente");
        }
        else
        {
            return Json("Error");
        }
    }
    else
    {
        return Json("Error: Expediente no encontrado");
    }
}

    public JsonResult EliminarExpediente(int expedienteID)
    {
        var expediente = _context.Expedientes.Find(expedienteID);
        _context.Remove(expediente);
        _context.SaveChanges();

        return Json(true);
    }

    
}