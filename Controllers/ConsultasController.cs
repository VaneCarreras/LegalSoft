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

public class ConsultasController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    //CONSTRUCTOR
    public ConsultasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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


    return View();
}

public JsonResult ListadoConsultas(int? id)
{
    // Obtener la lista de consultas
    var consultas = _context.Consultas.ToList();

    // Filtrar por ID si es proporcionado
    if (id.HasValue)
    {
        consultas = consultas.Where(c => c.ConsultaID == id.Value).ToList();
    }

    // Crear una lista de consultas para mostrar
    List<VistaConsulta> consultasMostrar = new List<VistaConsulta>();

    foreach (var consulta in consultas)
    {
        // Obtener el nombre completo del cliente
        var clienteNombre = _context.Clientes
            .Where(cli => cli.ClienteID == consulta.ClienteID)
            .Join(_context.Personas, cli => cli.PersonaID, p => p.PersonaID, (cli, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Cliente]";

        // Obtener el nombre completo del equipo
        var equipoNombre = _context.Equipos
            .Where(eq => eq.EquipoID == consulta.EquipoID)
            .Join(_context.Personas, eq => eq.PersonaID, p => p.PersonaID, (eq, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Equipo]";

        var consultaMostrar = new VistaConsulta
        {
            ConsultaID = consulta.ConsultaID,
            ClienteID = consulta.ClienteID,
            EquipoID = consulta.EquipoID,
            Descripcion = consulta.Descripcion,
            Fecha = consulta.Fecha,
            NombreCompletoCliente = clienteNombre, // <-- Este campo ahora existe
            NombreCompletoEquipo = equipoNombre    // <-- Este campo ahora existe
        };

        consultasMostrar.Add(consultaMostrar);
    }

    return Json(consultasMostrar);
}




public JsonResult BuscarConsultas(string NombreCompletoClienteBuscar, string NombreCompletoEquipoBuscar)
{
    // Obtener la lista de consultas
    var consultas = _context.Consultas.ToList();

    // Crear una lista de consultas para mostrar
    List<VistaConsulta> consultasMostrar = new List<VistaConsulta>();

    foreach (var consulta in consultas)
    {
        // Obtener el nombre completo del cliente
        var clienteNombre = _context.Clientes
            .Where(cli => cli.ClienteID == consulta.ClienteID)
            .Join(_context.Personas, cli => cli.PersonaID, p => p.PersonaID, (cli, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Cliente]";

        // Obtener el nombre completo del equipo
        var equipoNombre = _context.Equipos
            .Where(eq => eq.EquipoID == consulta.EquipoID)
            .Join(_context.Personas, eq => eq.PersonaID, p => p.PersonaID, (eq, p) => p.NombreCompleto)
            .FirstOrDefault() ?? "[Sin Equipo]";

        var consultaMostrar = new VistaConsulta
        {
            ConsultaID = consulta.ConsultaID,
            ClienteID = consulta.ClienteID,
            EquipoID = consulta.EquipoID,
            Descripcion = consulta.Descripcion,
            Fecha = consulta.Fecha,
            NombreCompletoCliente = clienteNombre,
            NombreCompletoEquipo = equipoNombre
        };

        consultasMostrar.Add(consultaMostrar);
    }

    // Ahora sí, aplicar el filtro sobre consultasMostrar, que **sí tiene** NombreCompletoCliente y NombreCompletoEquipo
    if (!string.IsNullOrEmpty(NombreCompletoClienteBuscar))
    {
        consultasMostrar = consultasMostrar
            .Where(x => x.NombreCompletoCliente.ToLower().Contains(NombreCompletoClienteBuscar.ToLower()))
            .ToList();
    }

    if (!string.IsNullOrEmpty(NombreCompletoEquipoBuscar))
    {
        consultasMostrar = consultasMostrar
            .Where(x => x.NombreCompletoEquipo.ToLower().Contains(NombreCompletoEquipoBuscar.ToLower()))
            .ToList();
    }

    return Json(consultasMostrar);
}





    public JsonResult GuardarNuevaConsulta(int consultaID, int clienteID, int equipoID, DateOnly fecha, string? descripcion, string? nombreCompletoCliente, string? nombreCompletoEquipo)
    {

        var error = 0;

         if (error == 0)

        {

            //4- GUARDAR
            var consulta = new Consulta
            {
                ConsultaID = consultaID,
                ClienteID = clienteID,
                EquipoID = equipoID,
                Fecha = fecha,
                Descripcion = descripcion,
            };
            _context.Add(consulta);
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

public JsonResult EditarConsulta(int consultaID, int clienteID, int equipoID, DateOnly fecha, string? descripcion, string? nombreCompletoCliente, string? nombreCompletoEquipo)
{
    // Buscar el cliente por el ID proporcionado
    var consultaEditar = _context.Consultas.SingleOrDefault(c => c.ConsultaID == consultaID);

    // Si el cliente existe, buscamos la persona relacionada
    if (consultaEditar != null)
    {
        
        // Si la persona existe, actualizamos sus datos
        if (consultaEditar != null)
        {
            consultaEditar.ClienteID = clienteID;
            consultaEditar.EquipoID = equipoID;
            consultaEditar.Descripcion = descripcion;
            consultaEditar.Fecha = fecha;

            // Guardamos los cambios en la base de datos
            _context.SaveChanges();

            return Json("Consulta actualizada correctamente");
        }
        else
        {
            return Json("Error");
        }
    }
    else
    {
        return Json("Error: Consulta no encontrado");
    }
}

    public JsonResult EliminarConsulta(int consultaID)
    {
        var consulta = _context.Consultas.Find(consultaID);
        _context.Remove(consulta);
        _context.SaveChanges();

        return Json(true);
    }

    
}


