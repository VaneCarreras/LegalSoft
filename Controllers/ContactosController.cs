
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
            string body = $"Nombre: {modelo.Nombre}\nEmail: {modelo.Email}\nMensaje:\n{modelo.Mensaje}";

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
