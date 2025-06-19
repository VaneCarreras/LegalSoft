using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LegalSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LegalSoft.Data;

namespace LegalSoft.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;
 

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _rolManager = rolManager;
    }

    
    public IActionResult Graficos()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

public IActionResult InicioSistema()
        {   //BUSCAMOS EL USUARIO ACTUAL
            var usuarioactual = _userManager.GetUserId(HttpContext.User);

            //REDIRECCIONAMOS AL INDEX DE home
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
    {
        await CrearRol();
        return View();
    }

    public async Task<JsonResult> CrearRol(){
        var clienteRolExiste = _context.Roles.Where(r => r.Name == "Cliente").SingleOrDefault();
        var equipoRolExiste = _context.Roles.Where(r => r.Name == "Equipo").SingleOrDefault();
        var rolAdminExiste = _context.Roles.Where(r => r.Name == "Administrador").SingleOrDefault();

        if (clienteRolExiste == null) {
            var rol = await _rolManager.CreateAsync(new IdentityRole("Cliente"));
        }
        if (equipoRolExiste == null) {
            var rol = await _rolManager.CreateAsync(new IdentityRole("Equipo"));
        }
        if (rolAdminExiste == null) {
            var rol = await _rolManager.CreateAsync(new IdentityRole("Administrador"));
        }

        //Crear usuario admin si no existe
        bool userAdminCreado = false;
        var admin = _context.Users.Where(u => u.Email == "admin@legalsoft.com").SingleOrDefault();

        if(admin == null){
            var userAdmin = new IdentityUser { UserName = "admin@legalsoft.com", Email = "admin@legalsoft.com" };
            var result = await _userManager.CreateAsync(userAdmin, "admin@legalsoft");

            await _userManager.AddToRoleAsync(userAdmin, "Administrador");
            userAdminCreado = result.Succeeded;
        }


        return Json(userAdminCreado);
    }



    // Ejemplo URL: /Admin/AsignarRol?email=admin@email.com&rol=administrador
    //http://localhost:5025/Home/AsignarRol?email=sofia@gmail.com&rol=equipo
    public async Task<IActionResult> AsignarRol(string email, string rol)
    {
        var usuario = await _userManager.FindByEmailAsync(email);

        if (usuario == null)
        {
            return Content($"No se encontró ningún usuario con el email {email}");
        }

        

        await _userManager.AddToRoleAsync(usuario, rol);

        return Content($"El usuario {email} fue asignado al rol '{rol}' correctamente.");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
