using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LegalSoft.Data;
using Microsoft.AspNetCore.Authorization;

namespace LegalSoft.Controllers;
    [Authorize(Roles = "Administrador")]

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
    var datos = (from consulta in _context.Consultas
                 join equipo in _context.Equipos on consulta.EquipoID equals equipo.EquipoID
                 join persona in _context.Personas on equipo.PersonaID equals persona.PersonaID
                 where consulta.Fecha.Month == mes 
                    && consulta.Fecha.Year == anio
                    && consulta.Habilitado == true
                 group consulta by new { persona.NombreCompleto } into g
                 select new
                 {
                     nombreEstado = g.Key.NombreCompleto,
                     cantidad = g.Count()
                 })
                 .ToList();




            resultado.AddRange(datos);
        }
        else if (tipo == "expedientes")
        {
          
          
    var datos = (from expediente in _context.Expedientes
                 join equipo in _context.Equipos on expediente.EquipoID equals equipo.EquipoID
                 join persona in _context.Personas on equipo.PersonaID equals persona.PersonaID
                 where expediente.FechaInicio.Month == mes && expediente.FechaInicio.Year == anio && expediente.Habilitado == true

                 group expediente by new { persona.NombreCompleto } into g
                 select new
                 {
                     nombreEstado = g.Key.NombreCompleto,
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
