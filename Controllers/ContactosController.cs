using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;

namespace LegalSoft.Controllers;

public class ContactosController : Controller
{
    private readonly ILogger<ContactosController> _logger;

    public ContactosController(ILogger<ContactosController> logger)
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

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
