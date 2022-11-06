using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MajaTours.Models;
using MajaTours.Data;

namespace MajaTours.Controllers;

public class HomeController : Controller
{
    //CONSTRUCTOR PARA INYECTAR A LA BASE DE DATOS.
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
    // FIN
    

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //------ PAGINA NO ENCONTRADA ----
        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    //---------------

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
