using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LegalSoft.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Authorization;


namespace LegalSoft.Controllers;
[Authorize]

public class TurnosController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    //CONSTRUCTOR
    public TurnosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    [HttpGet]
    public JsonResult GetClientes()
    {
        // Obtener personas
        var personas = _context.Personas
            .Select(p => new { p.PersonaID, p.NombreCompleto })
            .ToList();

        // Obtener clientes
        var clientes = _context.Clientes
            .Select(c => new { c.ClienteID, c.PersonaID })
            .ToList();

        // Unir manualmente clientes con personas por PersonaID
        var clientesConNombre = (from c in clientes
                                 join p in personas on c.PersonaID equals p.PersonaID
                                 select new
                                 {
                                     ClienteID = c.ClienteID,
                                     NombreCompleto = p.NombreCompleto
                                 })
                                 .OrderBy(c => c.NombreCompleto)
                                 .ToList();

        return Json(clientesConNombre);
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



    public IActionResult Index()
    {


        return View();
    }

    [HttpGet]
public JsonResult GetEvents()
{
    var turnos = (from t in _context.Turnos
                  join c in _context.Clientes on t.ClienteID equals c.ClienteID
                  join e in _context.Equipos on t.EquipoID equals e.EquipoID
                  join pc in _context.Personas on c.PersonaID equals pc.PersonaID
                  join pe in _context.Personas on e.PersonaID equals pe.PersonaID
                  select new
                  {
                      id = t.TurnoID,
                      title = $"{pc.NombreCompleto}",
                      start = t.FechaHora,
                      turnoID = t.TurnoID,
                      clienteID = t.ClienteID,
                      equipoID = t.EquipoID,
                      estado = (int)t.Estado
                  }).ToList();

    return Json(turnos);
}


    [HttpPost]
    public JsonResult SaveEvent(Turno turno)
    {
        if (turno.TurnoID == 0)
        {
            _context.Turnos.Add(turno);
        }
        else
        {
            var turnoDb = _context.Turnos.FirstOrDefault(t => t.TurnoID == turno.TurnoID);
            if (turnoDb == null) return Json(new { success = false });

            turnoDb.ClienteID = turno.ClienteID;
            turnoDb.EquipoID = turno.EquipoID;
            turnoDb.FechaHora = turno.FechaHora;
            turnoDb.Estado = turno.Estado;
        }

        _context.SaveChanges();
        return Json(new { success = true });
    }

    [HttpGet]
public IActionResult ImprimirTicket(int turnoID)
{
    var turno = _context.Turnos
        .FirstOrDefault(t => t.TurnoID == turnoID);
    if (turno == null) return NotFound();

    var cliente = _context.Clientes
        .Where(c => c.ClienteID == turno.ClienteID)
        .Join(_context.Personas, c => c.PersonaID, p => p.PersonaID,
              (c, p) => p.NombreCompleto)
        .FirstOrDefault();

    var equipo = _context.Equipos
        .Where(e => e.EquipoID == turno.EquipoID)
        .Join(_context.Personas, e => e.PersonaID, p => p.PersonaID,
              (e, p) => p.NombreCompleto)
        .FirstOrDefault();

    using var ms = new MemoryStream();
    var pageSize = new iTextSharp.text.Rectangle(
        iTextSharp.text.PageSize.A4.Width / 2,
        iTextSharp.text.PageSize.A4.Height / 3
    );

    var doc = new Document(pageSize, 20f, 20f, 20f, 20f);
    var writer = PdfWriter.GetInstance(doc, ms);
    doc.Open();

    // Colores y fuente
    var greenPastel = new BaseColor(119, 221, 119); // #77DD77
    var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "calibri.ttf");
    var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    var font = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.NORMAL, greenPastel);
    var titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD, greenPastel);

    // Contenedor (recuadro)
    var table = new PdfPTable(1)
    {
        WidthPercentage = 100
    };

    // Logo (centro superior)
    string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "logo.png");
    if (System.IO.File.Exists(logoPath))
    {
        var logo = iTextSharp.text.Image.GetInstance(logoPath);
        logo.ScaleToFit(80f, 80f);
        logo.Alignment = Element.ALIGN_CENTER;
        PdfPCell logoCell = new PdfPCell(logo)
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_CENTER,
            PaddingBottom = 10f
        };
        table.AddCell(logoCell);
    }

    // Título
    PdfPCell titleCell = new PdfPCell(new Phrase("TICKET DE TURNO", titleFont))
    {
        Border = Rectangle.NO_BORDER,
        HorizontalAlignment = Element.ALIGN_CENTER,
        PaddingBottom = 20f
    };
    table.AddCell(titleCell);

    // Info turno
    void AddLine(string label, string value)
    {
        var p = new Phrase($"{label}: {value}", font);
        var cell = new PdfPCell(p)
        {
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 10f,
            HorizontalAlignment = Element.ALIGN_LEFT
        };
        table.AddCell(cell);
    }

    AddLine("Cliente", cliente ?? "Desconocido");
    AddLine("Equipo", equipo ?? "Desconocido");
    AddLine("Fecha y Hora", turno.FechaHora.ToString("dd/MM/yyyy HH:mm"));

    // Agregar tabla al documento
    doc.Add(table);

    doc.Close();
return File(ms.ToArray(), "application/pdf", $"Ticket-Turno-{turnoID}.pdf");
}

}
