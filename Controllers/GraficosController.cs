using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LegalSoft.Data;

namespace LegalSoft.Controllers;

public class GraficosController : Controller
{
    private readonly ApplicationDbContext _context;

    public GraficosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public JsonResult GetDatosEstado(string tipo, int mes, int anio)
    {
        List<object> resultado = new List<object>();

        if (tipo == "consultas")
        {
            var datos = _context.Consultas
                .Where(c => c.Fecha.Month == mes && c.Fecha.Year == anio)
                .GroupBy(c => c.EstadoConsulta)
                .Select(g => new {
                    nombreEstado = g.Key,
                    cantidad = g.Count()
                })
                .ToList();

            resultado.AddRange(datos);
        }
        else if (tipo == "expedientes")
        {
            var datos = _context.Expedientes
                .Where(e => e.FechaInicio.Month == mes && e.FechaInicio.Year == anio)
                .GroupBy(e => e.EstadoExpediente)
                .Select(g => new {
                    nombreEstado = g.Key,
                    cantidad = g.Count()
                })
                .ToList();

            resultado.AddRange(datos);
        }
        else if (tipo == "turnos")
        {
            var datos = _context.Turnos
                .Where(t => t.FechaHora.Month == mes && t.FechaHora.Year == anio)
                .GroupBy(t => t.Estado)
                .Select(g => new {
                    nombreEstado = g.Key,
                    cantidad = g.Count()
                })
                .ToList();

            resultado.AddRange(datos);
        }
        else if (tipo == "pendientes")
        {
            var datos = _context.Pendientes
                .Where(p => p.FechaHora.Month == mes && p.FechaHora.Year == anio)
                .GroupBy(p => p.Estado)
                .Select(g => new {
                    nombreEstado = g.Key,
                    cantidad = g.Count()
                })
                .ToList();

            resultado.AddRange(datos);
        }

        return Json(resultado);
    }
}
