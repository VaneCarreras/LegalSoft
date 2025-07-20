
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using LegalSoft.Models;

namespace LegalSoft.Controllers;

public class ContactosController : Controller
{
    private readonly ILogger<ContactosController> _logger;

    public ContactosController(ILogger<ContactosController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public JsonResult EnviarCorreo([FromBody] Contacto modelo)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Json(new { exito = false, errores });
        }

        try
        {
            var fromAddress = new MailAddress("vanecarreras91@gmail.com", "Formulario Web");
            var toAddress = new MailAddress("vanecarreras91@gmail.com", "Vanessa Carreras");
            const string fromPassword = "kqmohuwdrrimriqi";
            string subject = "Nuevo mensaje del formulario de contacto";
            string body = $"Nombre: {modelo.Nombre}\n" +
                          $"Email: {modelo.Email}\n" +
                          $"Área Seleccionada: {modelo.AreaSeleccionada}\n\n";

            if (!string.IsNullOrWhiteSpace(modelo.Empleador))
                body += $"Empleador: {modelo.Empleador}\n";

            if (!string.IsNullOrWhiteSpace(modelo.Horas))
                body += $"Horas: {modelo.Horas}\n";

            if (!string.IsNullOrWhiteSpace(modelo.Situacion))
                body += $"Situacion: {modelo.Situacion}\n";

            if (!string.IsNullOrWhiteSpace(modelo.Sueldo))
                body += $"Sueldo: {modelo.Sueldo}\n";

                
            if (!string.IsNullOrWhiteSpace(modelo.Vinculo))
                body += $"Vínculo Familiar: {modelo.Vinculo}\n";

            

            if (!string.IsNullOrWhiteSpace(modelo.Datos))
                body += $"Datos: {modelo.Datos}\n";

            if (!string.IsNullOrWhiteSpace(modelo.Datos2))
                body += $"Datos: {modelo.Datos2}\n";

            if (!string.IsNullOrWhiteSpace(modelo.Motivo))
                body += $"Motivo de Consulta: {modelo.Motivo}\n";

            if (!string.IsNullOrWhiteSpace(modelo.Detalle))
                body += $"Detalle: {modelo.Detalle}\n";


            if (!string.IsNullOrWhiteSpace(modelo.Delito))
                body += $"Delito: {modelo.Delito}\n";

            if (!string.IsNullOrWhiteSpace(modelo.Tipo))
                body += $"Tipo: {modelo.Tipo}\n";



            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };


            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            smtp.Send(message);

            return Json(new { exito = true, mensaje = "Mensaje enviado correctamente." });
        }
        catch (Exception ex)
        {
            return Json(new { exito = false, mensaje = $"Error al enviar el correo: {ex.Message}" });
        }
    }

}
