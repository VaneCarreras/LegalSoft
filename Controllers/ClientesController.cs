using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using LegalSoft.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LegalSoft.Controllers;

[Authorize]
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

        var clientes = _context.Clientes.ToList();


        if (id != null)
        {
            clientes = clientes.Where(c => c.ClienteID == id).ToList();

        }

        
        foreach (var cliente in clientes)
        {
            ////RELLENAR CON LOS DATOS
        }


        return Json(clientes);
    }

    public JsonResult GuardarNuevoCliente(string nroTipoDoc, string nombreCompleto, string direccion, string telefono, DateTime fechaNac)
    {
        int error = 0; 

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


            
            // else
            // {
            //     //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
            //     var clienteEditar = _context.Clientes.Where(c => c.ClienteID == clienteID).SingleOrDefault();

            //     var personaEditar = _context.Personas.Where(p => p.PersonaID == personaID).SingleOrDefault();
            //     if (personaEditar != null)
            //     {
            //         personaEditar.NombreCompleto = nombreCompleto;
            //         personaEditar.UsuarioID = usuarioID;
            //         personaEditar.NroTipoDoc = nroTipoDoc;
            //         personaEditar.NombreCompleto = nombreCompleto;
            //         personaEditar.Direccion = direccion;
            //         personaEditar.Telefono = telefono;
            //         personaEditar.FechaNac  = fechaNac;
            //         _context.SaveChanges();
            //     }

            //     if (clienteEditar != null)
            //     {
            //         clienteEditar.PersonaID = personaID; 
            //         _context.SaveChanges();
            //     }


            // }
        }

        return Json(error);
    }

    public JsonResult EliminarCliente(int clienteID)
    {
        var cliente = _context.Clientes.Find(clienteID);
        _context.Remove(cliente);
        _context.SaveChanges();

        return Json(true);
    }

}




///////////////////////////////////EL CONTROLADOR DE CLIENTES ESTA DE MODIFICAR, INVERTIR CREAR PERSONA 1ERO ANTES DE EQUIPO, CAMBIAR EN EQUIPO
///LOS JS ESTAN DE COMPLETAR, EL CREAR ES DIFERENTE Y HAY Q TERMINARLO Y EL EDITAR VA APARTE Y EN PERSONA//////////
