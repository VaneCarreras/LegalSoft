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
        nombreEstado = g.Key.ToString(),  
        cantidad = g.Count()
    })
    .ToList();


            resultado.AddRange(datos);
        }
        else if (tipo == "expedientes")
        {
            var datos = _context.Expedientes
    .Where(c => c.FechaInicio.Month == mes && c.FechaInicio.Year == anio)
    .GroupBy(c => c.EstadoExpediente)
    .Select(g => new {
        nombreEstado = g.Key.ToString(),  // Convierte enum a string legible
        cantidad = g.Count()
    })
    .ToList();


            resultado.AddRange(datos);
        }
        else if (tipo == "turnos")
        {
            var datos = _context.Turnos
    .Where(c => c.FechaHora.Month == mes && c.FechaHora.Year == anio)
    .GroupBy(c => c.Estado)
    .Select(g => new {
        nombreEstado = g.Key.ToString(),  // Convierte enum a string legible
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
