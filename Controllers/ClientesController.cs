using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
public JsonResult ListadoClientes(int? id)
    {
        //HACER ANTES LOS JS
        var clientes = _context.Clientes.Include(c => c.Persona).ToList();

        if (id != null)
        {
            clientes = clientes.Where(c => c.ClienteID == id).ToList();
        }

        var clientesMostrar = clientes
        .Select(c => new VistaCliente
        //
        {
            ClienteID = c.ClienteID,
            NombreCompleto = c.Persona.NombreCompleto,
            NroTipoDoc = c.Persona.NroTipoDoc,
            Direccion = c.Persona.Direccion,
            Telefono = c.Persona.Telefono,
            FechaNac = c.Persona.FechaNac
        })
        .OrderBy(c => c.NombreCompleto).ToList();



        return Json(clientesMostrar);
    }
    
public JsonResult BuscarClientes(string nombreCompleto, string nroTipoDoc)
        {
            var clientes = _context.Clientes
                .Include(c => c.Persona)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombreCompleto))
            {
                clientes = clientes.Where(c => c.Persona.NombreCompleto.ToLower().Contains(nombreCompleto.ToLower()));
            }

            if (!string.IsNullOrEmpty(nroTipoDoc))
            {
                clientes = clientes.Where(c => c.Persona.NroTipoDoc.Contains(nroTipoDoc));
            }

            var clientesMostrar = clientes.Select(c => new VistaCliente
            {
                ClienteID = c.ClienteID,
                NombreCompleto = c.Persona.NombreCompleto,
                NroTipoDoc = c.Persona.NroTipoDoc,
                Direccion = c.Persona.Direccion,
                Telefono = c.Persona.Telefono,
                FechaNac = c.Persona.FechaNac
            }).OrderBy(c => c.NombreCompleto).ToList();

            return Json(clientesMostrar);
        }


    public JsonResult GuardarNuevoCliente(string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac)
    {
        int error = 0; 
        string resultado = "";
        string usuarioID = "";

        if(error == 0)
        {        
            

                var persona = new Persona
                {                 
                    NombreCompleto = nombreCompleto,
                    UsuarioID = usuarioID,
                    NroTipoDoc = nroTipoDoc,
                    Direccion = direccion,
                    Telefono = telefono,
                    FechaNac = fechaNac,
                };
                _context.Add(persona);
                _context.SaveChanges();

                var cliente = new Cliente
                {
                    PersonaID = persona.PersonaID,
                };
                _context.Add(cliente);
                _context.SaveChanges();


            
            
        }

        return Json(resultado);
    }

    public JsonResult EliminarCliente(int ClienteID)
    {
        var cliente = _context.Clientes.Find(ClienteID);
        _context.Remove(cliente);
        _context.SaveChanges();

        return Json(true);
    }

[HttpPost]
public JsonResult EditarCliente(int ClienteID, string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateOnly fechaNac)
{
    // Buscar el cliente por el ID proporcionado
    var clienteEditar = _context.Clientes
                                .Include(c => c.Persona) // Incluir la relación Persona
                                .SingleOrDefault(c => c.ClienteID == ClienteID);

    // Si el cliente existe, actualizamos sus datos
    if (clienteEditar != null && clienteEditar.Persona != null)
    {
        // Actualizamos los datos de la persona asociada al cliente
        clienteEditar.Persona.NroTipoDoc = nroTipoDoc;
        clienteEditar.Persona.NombreCompleto = nombreCompleto;
        clienteEditar.Persona.Direccion = direccion;
        clienteEditar.Persona.Telefono = telefono;
        clienteEditar.Persona.FechaNac = fechaNac;

        // Guardamos los cambios en la base de datos
        _context.SaveChanges();

        return Json("Cliente actualizado correctamente");
    }
    else
    {
        // Si no se encuentra el cliente o su persona, devolvemos un error
        return Json("Error: Cliente no encontrado");
    }
}



}




///////////////////////////////////EL CONTROLADOR DE CLIENTES ESTA DE MODIFICAR, INVERTIR CREAR PERSONA 1ERO ANTES DE EQUIPO, 
///CAMBIAR EN EQUIPO
///LOS JS ESTAN DE COMPLETAR, EL CREAR ES DIFERENTE Y HAY Q TERMINARLO Y EL EDITAR VA APARTE Y EN PERSONA//////////
///EL CONTROLADOR DE PERSONA EN SQL FUNCIONA TODO Y EL DE CLIENTE TAMBIEN, SOLO QUE CLIENTE 
/////NO ESTA HECHO EL MOSTRAR Y TAMPOCO TIENEN NINGUNO JS
