using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using Microsoft.EntityFrameworkCore;
using LegalSoft.Data;

namespace LegalSoft.Controllers;

public class PendientesController : Controller
{
    private readonly ILogger<PendientesController> _logger;
    private readonly ApplicationDbContext _context;

    public PendientesController(ILogger<PendientesController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Obtener pendientes para el calendario
    [HttpGet]
    public JsonResult GetPendientes()
    {
        var pendientes = _context.Pendientes.Include(p => p.Equipo).ToList();
        var eventos = pendientes.Select(p => new
        {
            id = p.PendienteID,
            title = p.Motivo,
            start = p.FechaHora,
            motivo = p.Motivo,
            equipoID = p.EquipoID,
            recordatorio = p.RecordatorioAlert,
                estado = p.Estado 

        }).ToList();

        return Json(eventos);
    }

    [HttpPost]
public JsonResult SavePendiente([FromBody] Pendiente pendiente)
{
    if (pendiente.PendienteID == 0)
    {
        _context.Pendientes.Add(pendiente);
    }
    else
    {
        _context.Pendientes.Update(pendiente);
    }
    _context.SaveChanges();
    return Json(new { success = true, id = pendiente.PendienteID });
}


    // Eliminar pendiente
    [HttpPost]
    public JsonResult DeletePendiente(int id)
    {
        var pendiente = _context.Pendientes.Find(id);
        if (pendiente == null) return Json(new { success = false });

        _context.Pendientes.Remove(pendiente);
        _context.SaveChanges();
        return Json(new { success = true });
    }

    public JsonResult GetEquipos()
{
    // Obtener personas
    var personas = _context.Personas
        .Select(p => new { p.PersonaID, p.NombreCompleto })
        .ToList();

    // Obtener clientes
    var equipos = _context.Equipos
        .Select(c => new { c.EquipoID, c.PersonaID })
        .ToList();

    // Unir manualmente clientes con personas por PersonaID
    var equiposConNombre = (from c in equipos
                             join p in personas on c.PersonaID equals p.PersonaID
                             select new
                             {
                                 EquipoID = c.EquipoID,
                                 NombreCompleto = p.NombreCompleto
                             })
                             .OrderBy(c => c.NombreCompleto)
                             .ToList();

    return Json(equiposConNombre);
}
}
