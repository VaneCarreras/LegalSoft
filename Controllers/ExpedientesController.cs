using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
// using Microsoft.AspNetCore.Authorization;


namespace LegalSoft.Controllers;
// [Authorize]

public class ExpedientesController : Controller
{
    private readonly ILogger<ExpedientesController> _logger;

    public ExpedientesController(ILogger<ExpedientesController> logger)
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
