using MajaTours.Data;
using MajaTours.Data.Entities;
using MajaTours.Enums;
using MajaTours.Helpers;
using MajaTours.Models;
using Microsoft.AspNetCore.Mvc;
namespace MajaTours.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public AccountController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        //----------- RGISTRA NUEVOS USUARIOS ----------
            public async Task<IActionResult> Register()
            {
                AddUserViewModel model = new AddUserViewModel
                {
                    Id = Guid.Empty.ToString(),
                    UserType = UserType.User,
                };
                return View(model);
            }

            //--METODO POST.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Register(AddUserViewModel model)
            {
                if (ModelState.IsValid) 
                {
                     
                    User user = await _userHelper.AddUserAsync(model);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                        return View(model);
                    }
                    LoginViewModel loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel);
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(model);
            }
        // --- FIN  ----- 


        //------- PAGINA NO AUTORIZADAD --------
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}