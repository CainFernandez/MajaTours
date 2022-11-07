using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MajaTours.Models;
using MajaTours.Data;
using MajaTours.Data.Entities;
using Microsoft.EntityFrameworkCore;
using MajaTours.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MajaTours.Controllers;

public class HomeController : Controller
{
    //CONSTRUCTOR PARA INYECTAR A LA BASE DE DATOS.
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public HomeController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
    // FIN
    
    // CONTROLADOR DE PAGINA DE INICIO
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
            .OrderBy(p => p.Description)
            .ToListAsync();
            
            HomeViewModel model = new() { Products = products };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _context.TemporalSales
                .Where(ts => ts.User.Id == user.Id)
                .SumAsync(ts => ts.Quantity);
            }
            return View(model);
        }
    //--

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

    // CREACIÓN DEL CONTROLADOR PARA AGREGAR AL CARRITO DE COMPRAS
        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = new()
            {
                Product = product,
                Quantity = 1,
                User = user
            };
            _context.TemporalSales.Add(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    //---

    // CONTROLADOR DETALLES DE LOS PRODUCTOS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Product product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            AddProductToCartViewModel model = new()
            {
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Quantity = 1
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(AddProductToCartViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
        Product product = await _context.Products.FindAsync(model.Id);
        if (product == null)
        {
            return NotFound();
        }
        User user = await _userHelper.GetUserAsync(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }
        TemporalSale temporalSale = new()
        {
            Product = product,
            Quantity = model.Quantity,
            Remarks = model.Remarks,
            User = user
        };
        _context.TemporalSales.Add(temporalSale);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
        }
    //-----

    //CONTROLADOR PARA MOSTRAR EL CARRO DE COMPRA
        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            List<TemporalSale>? temporalSales = await _context.TemporalSales
            .Include(ts => ts.Product)
            .Where(ts => ts.User.Id == user.Id)
            .ToListAsync();
            ShowCartViewModel model = new()
            {
                User = user,
                TemporalSales = temporalSales,
            };
            return View(model);
        }

        //CONTROLADOR PARA DISMINUIR LA CANTIDAD DE COMPRA
        public async Task<IActionResult> DecreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }
            
            if (temporalSale.Quantity > 1)
            {
                temporalSale.Quantity--;
                _context.TemporalSales.Update(temporalSale);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ShowCart));
        }

        //CONTROLADOR PARA AUMENTAR LA CANTIDAD COMPRA.
        public async Task<IActionResult> IncreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }
            temporalSale.Quantity++;
            _context.TemporalSales.Update(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }

        //CONTROLADOR PARA ELIMINAR UN PRODUCTO EN EL CARRITO DE COMPRAS.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }
            _context.TemporalSales.Remove(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }
    //--------
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
