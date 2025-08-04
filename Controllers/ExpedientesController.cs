using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LegalSoft.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;

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
        clientesConNombre.Add(new { ClienteID = 0, NombreCompleto = "[SELECCIONAR]" });


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
        equiposConNombre.Add(new { EquipoID = 0, NombreCompleto = "[SELECCIONAR]" });


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
        ViewBag.Area = new SelectList(
        Enum.GetValues(typeof(Area))
            .Cast<Area>()
            .Select(e => new { Value = (int)e, Text = e.ToString().ToUpper() }),
        "Value",
        "Text"
);
        ViewBag.Dependencia = new SelectList(
        Enum.GetValues(typeof(Dependencia))
            .Cast<Dependencia>()
            .Select(e => new { Value = (int)e, Text = e.ToString().ToUpper() }),
        "Value",
        "Text"
);
        ViewBag.Ubicacion = new SelectList(
        Enum.GetValues(typeof(Ubicacion))
            .Cast<Ubicacion>()
            .Select(e => new { Value = (int)e, Text = e.ToString().ToUpper() }),
        "Value",
        "Text"
    );



        return View();
    }
[HttpPost]

    public JsonResult ListadoExpedientes(int pagina = 1, int tamanioPagina = 7, int? id = null, bool soloHabilitados = false)
    {
        // Obtener la lista de consultas
        var expedientes = _context.Expedientes.ToList();

        // Filtrar por ID si es proporcionado
        if (id.HasValue)
        {
            expedientes = expedientes.Where(c => c.ExpedienteID == id.Value).ToList();
        }

        if (soloHabilitados)
    {
        expedientes = expedientes.Where(c => c.Habilitado).ToList();
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
                UltimoDecreto = expediente.UltimoDecreto,
                FechaInicio = expediente.FechaInicio,
                NombreCompletoCliente = clienteNombre, // <-- Este campo ahora existe
                NombreCompletoEquipo = equipoNombre,
                LinkContenido = expediente.LinkContenido,
                EstadoExpediente = expediente.EstadoExpediente,
                EstadoExpedienteString = expediente.EstadoExpediente.ToString().ToUpper(),
                Area = expediente.Area,
                AreaString = expediente.Area.ToString().ToUpper(),
                Dependencia = expediente.Dependencia,
                DependenciaString = expediente.Dependencia.ToString().ToUpper(),
                Ubicacion = expediente.Ubicacion,
                UbicacionString = expediente.Ubicacion.ToString().ToUpper(),    // <-- Este campo ahora existe
                Habilitado = expediente.Habilitado
            };

            expedientesMostrar.Add(expedienteMostrar);
        }

        // Ordenar por nombre
        var expedientesOrdenados = expedientesMostrar.OrderBy(e => e.FechaInicio).ToList();

        // Calcular total de registros y páginas
        var totalRegistros = expedientesOrdenados.Count();
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanioPagina);

        // Obtener solo la página solicitada
        var expedientesPaginados = expedientesOrdenados
            .Skip((pagina - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .ToList();

        return Json(new
        {
            expedientes = expedientesPaginados,
            totalPaginas = totalPaginas
        });
        }




    public JsonResult BuscarExpedientes(string DniEquipoBuscar, string CaratulaBuscar)
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
                ClienteID = expediente.ClienteID,
                EquipoID = expediente.EquipoID,
                UltimoDecreto = expediente.UltimoDecreto,

                NombreCompletoCliente = clienteNombre,
                NombreCompletoEquipo = equipoNombre,
                LinkContenido = expediente.LinkContenido,
                EstadoExpediente = expediente.EstadoExpediente,
                EstadoExpedienteString = expediente.EstadoExpediente.ToString().ToUpper(),
                Area = expediente.Area,
                AreaString = expediente.Area.ToString().ToUpper(),
                Dependencia = expediente.Dependencia,
                DependenciaString = expediente.Dependencia.ToString().ToUpper(),
                Ubicacion = expediente.Ubicacion,
                UbicacionString = expediente.Ubicacion.ToString().ToUpper(),    // <-- Este campo ahora existe
                Habilitado = expediente.Habilitado,
            };

            expedientesMostrar.Add(expedienteMostrar);
        }

        // Ahora sí, aplicar el filtro sobre consultasMostrar, que **sí tiene** NombreCompletoCliente y NombreCompletoEquipo
        if (!string.IsNullOrEmpty(DniEquipoBuscar))
        {
            expedientesMostrar = expedientesMostrar
                .Where(x => x.NombreCompletoEquipo.ToLower().Contains(DniEquipoBuscar.ToLower()))
                .OrderBy(e => e.FechaInicio).ToList();
        }

        if (!string.IsNullOrEmpty(CaratulaBuscar))
        {
            expedientesMostrar = expedientesMostrar
                .Where(x => x.Caratula.ToLower().Contains(CaratulaBuscar.ToLower()))
                .OrderBy(e => e.FechaInicio).ToList();
        }

        return Json(expedientesMostrar);
    }





    public JsonResult GuardarNuevoExpediente(int expedienteID, int clienteID, int equipoID, DateOnly fechaInicio,  string? nombreCompletoCliente, string? nombreCompletoEquipo, EstadoExpediente estadoExpediente,  Area area, Dependencia dependencia, Ubicacion ubicacion, string? numero, string? caratula, string? ultimoDecreto, string? linkContenido)
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
                UltimoDecreto = ultimoDecreto,
                FechaInicio = fechaInicio,
                LinkContenido = linkContenido,
                EstadoExpediente = estadoExpediente,
                Area = area,
                Dependencia = dependencia,
                Ubicacion = ubicacion,
                Habilitado = true,

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

    public JsonResult EditarExpediente(int expedienteID, int clienteID, int equipoID, DateOnly fechaInicio,  string? numero, string? caratula, string? ultimoDecreto, string? linkContenido, string? nombreCompletoCliente, string? nombreCompletoEquipo, EstadoExpediente estadoExpediente, Area area, Dependencia dependencia, Ubicacion ubicacion)
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
                expedienteEditar.UltimoDecreto = ultimoDecreto;
                expedienteEditar.FechaInicio = fechaInicio;
                expedienteEditar.EstadoExpediente = estadoExpediente;
                                expedienteEditar.Area = area;
                expedienteEditar.Dependencia = dependencia;
                expedienteEditar.Ubicacion = ubicacion;


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


    public JsonResult BuscarDocumentos(int ExpedienteID)
    {
            Console.WriteLine($"[BuscarDocumentos] ExpedienteID recibido: {ExpedienteID}");

        List<VistaDocsExpediente> listaDocs = new List<VistaDocsExpediente>();

        var docs = _context.DocsExpediente
                           .Where(d => d.ExpedienteID == ExpedienteID)
                           .ToList();

        foreach (var doc in docs)
        {
            var vistaDoc = new VistaDocsExpediente
            {
                DocID = doc.DocID,
                NombreArchivo = doc.NombreArchivo,
                Base64 = Convert.ToBase64String(doc.Imagen)
            };
            listaDocs.Add(vistaDoc);
        }

        

        return Json(listaDocs);
    }

public JsonResult GuardarDocumento(string DocumentoAGuardar, string NombreArchivo, int ExpedienteID)
{
    try
    {
        var cantidadDocs = _context.DocsExpediente.Count(d => d.ExpedienteID == ExpedienteID);
        if (cantidadDocs >= 10)
            return Json(new { resultado = false, error = "Límite de documentos alcanzado." });

        if (string.IsNullOrEmpty(DocumentoAGuardar) || string.IsNullOrEmpty(NombreArchivo))
            return Json(new { resultado = false, error = "Faltan datos para guardar." });

        var parts = DocumentoAGuardar.Split(',');
        if (parts.Length < 2)
            return Json(new { resultado = false, error = "Formato Base64 inválido." });

        byte[] archivoBytes;
        try
        {
            archivoBytes = Convert.FromBase64String(parts[1]);
        }
        catch (FormatException fe)
        {
            return Json(new { resultado = false, error = "Error formato Base64: " + fe.Message });
        }

        var nuevoDoc = new DocsExpediente
        {
            ExpedienteID = ExpedienteID,
            Imagen = archivoBytes,
            NombreArchivo = NombreArchivo,
            TipoImg = Path.GetExtension(NombreArchivo)?.ToLower(),
            Descripcion = null
        };

        try
        {
            _context.DocsExpediente.Add(nuevoDoc);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            var errorMsg = "Error guardando en base: " + ex.Message;
            if (ex.InnerException != null)
            {
                errorMsg += " | Inner: " + ex.InnerException.Message;
            }
            return Json(new { resultado = false, error = errorMsg });
        }

        return Json(new { resultado = true });
    }
    catch (Exception ex)
    {
        return Json(new { resultado = false, error = "Error inesperado: " + ex.Message });
    }
}

[HttpPost]
public JsonResult EliminarDocumento(int DocID)
{
    bool resultado = false;

    try
    {
        var doc = _context.DocsExpediente.FirstOrDefault(d => d.DocID == DocID);
        if (doc != null)
        {
            _context.DocsExpediente.Remove(doc);
            _context.SaveChanges();
            resultado = true;
        }
    }
    catch (Exception ex)
    {
        resultado = false;
    }

    return Json(resultado);
}
public IActionResult ImprimirExpediente(int expedienteID)
{
    var expediente = _context.Expedientes
        .Where(e => e.ExpedienteID == expedienteID)
        .Join(_context.Clientes, e => e.ClienteID, c => c.ClienteID,
            (e, c) => new { Expediente = e, Cliente = c })
        .Join(_context.Personas, ec => ec.Cliente.PersonaID, p => p.PersonaID,
            (ec, p) => new { ec.Expediente, ClienteNombre = p.NombreCompleto })
        .FirstOrDefault();

    if (expediente == null) return NotFound();

    var equipoNombre = _context.Equipos
        .Where(eq => eq.EquipoID == expediente.Expediente.EquipoID)
        .Join(_context.Personas, e => e.PersonaID, p => p.PersonaID,
            (e, p) => p.NombreCompleto)
        .FirstOrDefault();

    using var ms = new MemoryStream();
    var doc = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
    var writer = PdfWriter.GetInstance(doc, ms);
    doc.Open();

    var green = new BaseColor(119, 221, 119);
    var pink = new BaseColor(255, 105, 180);
    var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "calibri.ttf");
    var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    var blackFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);
    var greenTitleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD, green);
    var pinkFooterFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL, pink);

    var table = new PdfPTable(1) { WidthPercentage = 100 };

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

    table.AddCell(new PdfPCell(new Phrase("INFORMACIÓN DE EXPEDIENTE:", greenTitleFont))
    {
        Border = Rectangle.NO_BORDER,
        HorizontalAlignment = Element.ALIGN_CENTER,
        PaddingBottom = 20f
    });

    void AddLine(string label, string value)
    {
        table.AddCell(new PdfPCell(new Phrase($"{label}: {value}", blackFont))
        {
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 5f,
            HorizontalAlignment = Element.ALIGN_LEFT
        });

        table.AddCell(new PdfPCell(new Phrase(" ", blackFont))
        {
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 5f
        });
    }

// Contenido de la Carátula en mayúsculas como un título aparte en negro
table.AddCell(new PdfPCell(new Phrase((expediente.Expediente.Caratula ?? "").ToUpper(), blackFont))
{
    Border = Rectangle.NO_BORDER,
    PaddingBottom = 10f,
    HorizontalAlignment = Element.ALIGN_LEFT
});

        // Línea vacía
        table.AddCell(new PdfPCell(new Phrase(" ", blackFont))
        {
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 5f
        });

        AddLine("Cliente", expediente.ClienteNombre ?? "Desconocido");
    AddLine("Área", expediente.Expediente.Area.ToString());
    AddLine("Dependencia", expediente.Expediente.Dependencia.ToString());
    AddLine("Ubicación", expediente.Expediente.Ubicacion.ToString());
    AddLine("Fecha de Inicio", expediente.Expediente.FechaInicio.ToString("dd/MM/yyyy"));

    // Contenido del último decreto en bloque separado
    table.AddCell(new PdfPCell(new Phrase("Contenido del Último Decreto:", blackFont))
    {
        Border = Rectangle.NO_BORDER,
        PaddingBottom = 2f,
        HorizontalAlignment = Element.ALIGN_LEFT
    });

    table.AddCell(new PdfPCell(new Phrase(expediente.Expediente.UltimoDecreto ?? "Sin contenido", blackFont))
    {
        Border = Rectangle.NO_BORDER,
        PaddingBottom = 10f,
        HorizontalAlignment = Element.ALIGN_LEFT
    });

    table.AddCell(new PdfPCell(new Phrase(" ", blackFont))
    {
        Border = Rectangle.NO_BORDER,
        PaddingBottom = 5f
    });

    doc.Add(table);

    // Pie de página con línea y texto rosa a la derecha
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

    return File(ms.ToArray(), "application/pdf", $"Expediente-{expedienteID}.pdf");
}

[HttpPost]
public JsonResult CambiarEstadoExpediente(int ExpedienteID, bool habilitar)
{
    try
    {
        var expediente = _context.Expedientes.Find(ExpedienteID);
        if (expediente != null)
        {
            expediente.Habilitado = habilitar;
            _context.SaveChanges();
            return Json(new { success = true });
        }

        return Json(new { success = false, message = "Expediente no encontrado." });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}


}


