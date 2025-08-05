using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace LegalSoft.Controllers;

// [Authorize]
public class ClientesController : Controller
{

    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public ClientesController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        

return View();
    }


    [HttpGet]
    public JsonResult ObtenerLocalidades()
    {
        var localidades = _context.Localidades
            .Select(l => new
            {
                localidadID = l.LocalidadID,
                localidadNombre = l.LocalidadNombre
            }).ToList();

        return Json(localidades);
    }
[HttpPost]

public JsonResult ListadoClientes(int pagina = 1, int tamanioPagina = 7, int? id = null, bool soloHabilitados = false)
{
    // Obtener todos los clientes
    var clientes = _context.Clientes.ToList();

    // Filtrar si se especifica un ID
    if (id.HasValue)
    {
        clientes = clientes.Where(c => c.ClienteID == id.Value).ToList();
    }

if (soloHabilitados)
    {
        clientes = clientes.Where(c => c.Habilitado).ToList();
    }

        // Lista que se va a mostrar
        List<VistaCliente> clientesMostrar = new List<VistaCliente>();

    foreach (var cliente in clientes)
    {
        var persona = _context.Personas
            .Include(p => p.Localidad)
            .SingleOrDefault(p => p.PersonaID == cliente.PersonaID);

        if (persona != null)
        {
            clientesMostrar.Add(new VistaCliente
            {
                ClienteID = cliente.ClienteID,
                NombreCompleto = persona.NombreCompleto,
                NroTipoDoc = persona.NroTipoDoc,
                Direccion = persona.Direccion,
                Telefono = persona.Telefono,
                FechaNac = persona.FechaNac,
                LocalidadNombre = persona.Localidad?.LocalidadNombre,
                Habilitado = cliente.Habilitado,
            });
        }
    }

    // Ordenar por nombre
    var clientesOrdenados = clientesMostrar.OrderBy(c => c.NombreCompleto).ToList();

    // Calcular total de registros y páginas
    var totalRegistros = clientesOrdenados.Count();
    var totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanioPagina);

    // Obtener solo la página solicitada
    var clientesPaginados = clientesOrdenados
        .Skip((pagina - 1) * tamanioPagina)
        .Take(tamanioPagina)
        .ToList();

    return Json(new
    {
        clientes = clientesPaginados,
        totalPaginas = totalPaginas
    });
}


[HttpPost]

    public JsonResult BuscarClientes(string nombreCompleto, string nroTipoDoc, bool soloHabilitados= false)
    {
        var personas = _context.Personas.Include(p => p.Localidad).ToList();
        if (soloHabilitados)
{
    personas = personas
        .Where(p => _context.Clientes.Any(c => c.PersonaID == p.PersonaID && c.Habilitado))
        .ToList();
}


        if (!string.IsNullOrEmpty(nombreCompleto))
        {
            personas = personas.Where(c => c.NombreCompleto.ToLower().Contains(nombreCompleto.ToLower())).ToList();
        }

        if (!string.IsNullOrEmpty(nroTipoDoc))
        {
            personas = personas.Where(c => c.NroTipoDoc.Contains(nroTipoDoc)).ToList();
        }

        List<VistaCliente> clientesMostrar = new List<VistaCliente>();
        foreach (var persona in personas.OrderBy(c => c.NombreCompleto))
        {
            var cliente = _context.Clientes.Where(c => c.PersonaID == persona.PersonaID).SingleOrDefault();
            if (cliente != null)
            {
                var clienteMostrar = new VistaCliente
                {
                    ClienteID = cliente.ClienteID,
                    NombreCompleto = persona.NombreCompleto,
                    NroTipoDoc = persona.NroTipoDoc,
                    Direccion = persona.Direccion,
                    Telefono = persona.Telefono,
                    FechaNac = persona.FechaNac,
                    LocalidadNombre = persona.Localidad?.LocalidadNombre,
                Habilitado = cliente.Habilitado,


                };
                clientesMostrar.Add(clienteMostrar);
            }
        }

        return Json(clientesMostrar);
    }

[HttpPost]

    public JsonResult GuardarNuevoCliente(string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac, int localidadID, string localidadNombre)
    {
        int error = 0;
        string resultado = "";
        string usuarioID = "";

        if (error == 0)
        {


            var persona = new Persona
            {
                NombreCompleto = nombreCompleto,
                UsuarioID = usuarioID,
                NroTipoDoc = nroTipoDoc,
                Direccion = direccion,
                Telefono = telefono,
                FechaNac = fechaNac,
                LocalidadID = localidadID,

            };
            _context.Add(persona);
            _context.SaveChanges();

            var cliente = new Cliente
            {
                PersonaID = persona.PersonaID,
                                Habilitado = true,

            };
            _context.Add(cliente);
            _context.SaveChanges();




        }

        return Json(resultado);
    }
[HttpPost]

    public JsonResult EliminarCliente(int ClienteID)
    {
        var cliente = _context.Clientes.Find(ClienteID);
        _context.Remove(cliente);
        _context.SaveChanges();

        return Json(true);
    }

    [HttpPost]
public JsonResult CambiarEstadoCliente(int ClienteID, bool habilitar)
{
    try
    {
        var cliente = _context.Clientes.Find(ClienteID);
        if (cliente != null)
        {
            cliente.Habilitado = habilitar;
            _context.SaveChanges();
            return Json(new { success = true });
        }

        return Json(new { success = false, message = "Cliente no encontrado." });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}


    [HttpPost]
    public JsonResult EditarCliente(int ClienteID, string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac, int localidadID)
    {
        // Buscar el cliente por el ID proporcionado
        var clienteEditar = _context.Clientes.SingleOrDefault(c => c.ClienteID == ClienteID);

        // Si el cliente existe, buscamos la persona relacionada
        if (clienteEditar != null)
        {
            var personaEditar = _context.Personas.SingleOrDefault(p => p.PersonaID == clienteEditar.PersonaID);

            // Si la persona existe, actualizamos sus datos
            if (personaEditar != null)
            {
                personaEditar.NroTipoDoc = nroTipoDoc;
                personaEditar.NombreCompleto = nombreCompleto;
                personaEditar.Direccion = direccion;
                personaEditar.Telefono = telefono;
                personaEditar.FechaNac = fechaNac;
                personaEditar.LocalidadID = localidadID;


                // Guardamos los cambios en la base de datos
                _context.SaveChanges();

                return Json("Cliente actualizado correctamente");
            }
            else
            {
                return Json("Error: Persona asociada no encontrada");
            }
        }
        else
        {
            return Json("Error: Cliente no encontrado");
        }
    }







    ///////////////////////////////////EL CONTROLADOR DE CLIENTES ESTA DE MODIFICAR, INVERTIR CREAR PERSONA 1ERO ANTES DE EQUIPO, 
    ///CAMBIAR EN EQUIPO
    ///LOS JS ESTAN DE COMPLETAR, EL CREAR ES DIFERENTE Y HAY Q TERMINARLO Y EL EDITAR VA APARTE Y EN PERSONA//////////
    ///EL CONTROLADOR DE PERSONA EN SQL FUNCIONA TODO Y EL DE CLIENTE TAMBIEN, SOLO QUE CLIENTE 
    /////NO ESTA HECHO EL MOSTRAR Y TAMPOCO TIENEN NINGUNO JS


    //CAMBIAR LISTADO Y EDITAR




[HttpGet]
    public JsonResult BuscarImagenes(int ClienteID)
    {
        List<VistaImagenCliente> listaImagenCliente = new List<VistaImagenCliente>();

        var imagenesCli = (from o in _context.ImagenCliente where o.ClienteID == ClienteID select o).ToList();

        foreach (var item in imagenesCli)
        {
            string returnValue = System.Convert.ToBase64String(item.Imagen);

            var imagenCliente = new VistaImagenCliente
            {
                ImgClientesID = item.ImagenClienteID,
                Base64 = returnValue
            };
            listaImagenCliente.Add(imagenCliente);
        }

        return Json(listaImagenCliente);
    }

    
[HttpPost]

public JsonResult GuardarImagen(string ImagenAGuardar, int ClienteID)
{
    var resultado = false;

    try
    {
        var cantidadImagenes = _context.ImagenCliente.Count(o => o.ClienteID == ClienteID);
        if (cantidadImagenes < 3)
        {
            if (!string.IsNullOrEmpty(ImagenAGuardar))
            {
                var base64Data = ImagenAGuardar.Split(',')[1];
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                using (var ms = new MemoryStream(imageBytes))
                using (var originalImage = Image.FromStream(ms))
                {
                    int maxWidth = 350;
                    int maxHeight = 350;

                    // Escalar proporcionalmente
                    double ratioX = (double)maxWidth / originalImage.Width;
                    double ratioY = (double)maxHeight / originalImage.Height;
                    double ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(originalImage.Width * ratio);
                    int newHeight = (int)(originalImage.Height * ratio);

                    using (var resizedImage = new Bitmap(newWidth, newHeight))
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);

                        using (var resizedMs = new MemoryStream())
                        {
                            resizedImage.Save(resizedMs, ImageFormat.Jpeg); // o ImageFormat.Png
                            byte[] resizedBytes = resizedMs.ToArray();

                            var imagenCli = new ImagenCliente
                            {
                                Imagen = resizedBytes,
                                ClienteID = ClienteID
                            };

                            _context.ImagenCliente.Add(imagenCli);
                            _context.SaveChanges();

                            resultado = true;
                        }
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        resultado = false;
    }

    return Json(resultado);
}
[HttpPost]

    public JsonResult EliminarImagenCliente(int ImgClientesID)
    {
        bool resultado = true;

        ImagenCliente imagenCliente = _context.ImagenCliente.Find(ImgClientesID);

        _context.ImagenCliente.Remove(imagenCliente);
        _context.SaveChanges();

        return Json(resultado);
    }
}







    

