using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
// using Microsoft.AspNetCore.Authorization;


namespace LegalSoft.Controllers;
// [Authorize]

public class RecordatoriosController : Controller
{
    private readonly ILogger<RecordatoriosController> _logger;

    public RecordatoriosController(ILogger<RecordatoriosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public JsonResult SaveEvent(Event e)
        {
            // Aquí puedes guardar el evento en la base de datos
            return Json(new { success = true });
        }

     public class Event
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }   
}