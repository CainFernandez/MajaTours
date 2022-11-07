using MajaTours.Data;
using MajaTours.Data.Entities;
using MajaTours.Helpers;
using MajaTours.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MajaTours.Controllers
{
    [Authorize(Roles="Admin")]
    public class UsersController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        //------------ INYECCIÓN A LA BASE DE DATOS -----------
        public UsersController(IUserHelper userHelper, DataContext context)
            {
            _userHelper = userHelper;
            _context = context;
        }
        //------------------
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // ---- CREACIÓN DE NUEVO ADMINISTRADOR ------//
            public async Task<IActionResult> Create()
            {
                AddUserViewModel model = new AddUserViewModel
                {
                    Id = Guid.Empty.ToString(),
                    UserType = Enums.UserType.Admin
                };
                return View(model);
            }

            //METODO POST
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(AddUserViewModel model)
            {
                if (ModelState.IsValid)
                {
                    User user = await _userHelper.AddUserAsync(model);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                        return View(model);
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            } 
        // --------------

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }            
    }
}