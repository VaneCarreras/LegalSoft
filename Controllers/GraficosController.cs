using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LegalSoft.Data;
using Microsoft.AspNetCore.Authorization;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
    .Select(g => new
    {
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
                .Select(g => new
                {
                    nombreEstado = g.Key,
                    cantidad = g.Count()
                })
                .ToList();

            resultado.AddRange(datos);
        }

        return Json(resultado);
    }
    


    [HttpGet]
public IActionResult ImprimirInforme(int mes, int anio)
{
    using var ms = new MemoryStream();
    var doc = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
    var writer = PdfWriter.GetInstance(doc, ms);
    doc.Open();

    // Colores y fuentes
    var green = new BaseColor(119, 221, 119);
    var pink = new BaseColor(255, 105, 180);
    var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "calibri.ttf");
    var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    var blackFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);
    var greenTitleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD, green);
    var sectionFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD, BaseColor.Black);
    var pinkFooterFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL, pink);

    var table = new PdfPTable(1) { WidthPercentage = 100 };

    // Logo
    string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "logo.png");
    if (System.IO.File.Exists(logoPath))
    {
        var logo = iTextSharp.text.Image.GetInstance(logoPath);
        logo.ScaleToFit(80f, 80f);
        logo.Alignment = Element.ALIGN_CENTER;
        table.AddCell(new PdfPCell(logo)
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_CENTER,
            PaddingBottom = 10f
        });
    }

    // Título
    table.AddCell(new PdfPCell(new Phrase("INFORME DE ACTIVIDADES", greenTitleFont))
    {
        Border = Rectangle.NO_BORDER,
        HorizontalAlignment = Element.ALIGN_CENTER,
        PaddingBottom = 20f
    });

    void AddSectionTitle(string title)
    {
        table.AddCell(new PdfPCell(new Phrase(title, sectionFont))
        {
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 10f,
            HorizontalAlignment = Element.ALIGN_LEFT
        });
    }

    void AddLine(string name, int cantidad)
    {
        table.AddCell(new PdfPCell(new Phrase($"{name}: {cantidad}", blackFont))
        {
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 5f,
            HorizontalAlignment = Element.ALIGN_LEFT
        });
    }

    // === CONSULTAS POR ABOGADO ===
    AddSectionTitle("Consultas por Abogado");
    var consultas = (from consulta in _context.Consultas
                     join equipo in _context.Equipos on consulta.EquipoID equals equipo.EquipoID
                     join persona in _context.Personas on equipo.PersonaID equals persona.PersonaID
                     where consulta.Fecha.Month == mes && consulta.Fecha.Year == anio && consulta.Habilitado
                     group consulta by persona.NombreCompleto into g
                     select new { Nombre = g.Key, Cantidad = g.Count() }).ToList();

    if (consultas.Any())
        consultas.ForEach(c => AddLine(c.Nombre, c.Cantidad));
    else
        AddLine("Sin registros", 0);

    table.AddCell(new PdfPCell(new Phrase(" ", blackFont)) { Border = Rectangle.NO_BORDER, PaddingBottom = 10f });

    // === EXPEDIENTES POR ABOGADO ===
    AddSectionTitle("Expedientes por Abogado");
    var expedientes = (from expediente in _context.Expedientes
                       join equipo in _context.Equipos on expediente.EquipoID equals equipo.EquipoID
                       join persona in _context.Personas on equipo.PersonaID equals persona.PersonaID
                       where expediente.FechaInicio.Month == mes && expediente.FechaInicio.Year == anio && expediente.Habilitado
                       group expediente by persona.NombreCompleto into g
                       select new { Nombre = g.Key, Cantidad = g.Count() }).ToList();

    if (expedientes.Any())
        expedientes.ForEach(e => AddLine(e.Nombre, e.Cantidad));
    else
        AddLine("Sin registros", 0);

    table.AddCell(new PdfPCell(new Phrase(" ", blackFont)) { Border = Rectangle.NO_BORDER, PaddingBottom = 10f });

    // === TURNOS POR ESTADO ===
    AddSectionTitle("Turnos por Estado");
    var turnos = _context.Turnos
        .Where(t => t.FechaHora.Month == mes && t.FechaHora.Year == anio)
        .GroupBy(t => t.Estado)
        .Select(g => new { Nombre = g.Key.ToString(), Cantidad = g.Count() }).ToList();

    if (turnos.Any())
        turnos.ForEach(t => AddLine(t.Nombre, t.Cantidad));
    else
        AddLine("Sin registros", 0);

    table.AddCell(new PdfPCell(new Phrase(" ", blackFont)) { Border = Rectangle.NO_BORDER, PaddingBottom = 10f });

    // === PENDIENTES POR ESTADO ===
    AddSectionTitle("Pendientes por Estado");
    var pendientes = _context.Pendientes
        .Where(p => p.FechaHora.Month == mes && p.FechaHora.Year == anio)
        .GroupBy(p => p.Estado)
        .Select(g => new { Nombre = g.Key, Cantidad = g.Count() }).ToList();

    if (pendientes.Any())
        pendientes.ForEach(p => AddLine(p.Nombre, p.Cantidad));
    else
        AddLine("Sin registros", 0);

    doc.Add(table);

    // Footer
    var cb = writer.DirectContent;
    cb.SetLineWidth(1f);
    cb.SetColorStroke(green);
    cb.MoveTo(30, 40);
    cb.LineTo(doc.PageSize.Width - 30, 40);
    cb.Stroke();

    var footer = new ColumnText(cb);
    footer.SetSimpleColumn(new Phrase("caporaliab.com", pinkFooterFont),
        30, 25, doc.PageSize.Width - 30, 40, 10, Element.ALIGN_RIGHT);
    footer.Go();

    doc.Close();

    return File(ms.ToArray(), "application/pdf", $"Informe-{mes}-{anio}.pdf");
}

}
