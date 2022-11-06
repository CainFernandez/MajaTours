using MajaTours.Data.Entities;
using MajaTours.Enums;
using MajaTours.Helpers;

namespace MajaTours.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        //############### INYECCIÓN A LA BASE DE DATOS ###########
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
             _userHelper = userHelper;

        } 
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // crea la base datos
            await CheckCategoriesAsync();  
            await CheckRolesAsync();
            await CheckUserAsync("Abel", "Pérez", "MajaTours@gmail.com", "322 311 4620", UserType.Admin); 
            await CheckUserAsync("Manuel", "Diaz", "Manuel@gmail.com", "6857565", UserType.User);     
        }
        private async Task<User> CheckUserAsync(
            string firstName,
            string lastName,
            string email,
            string phone,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    UserType = userType,
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }
        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        // ############### Agregando Categorias al alimentador de BD ###############
        private async Task CheckCategoriesAsync()
        {
            if(!_context.categories.Any()) // Any devuelve verdadero.
            {
                _context.categories.Add(new Category{Name = "Gastronomia"});
                _context.categories.Add(new Category{Name = "Playas"});
                _context.categories.Add(new Category{Name = "Naturaleza"});
                _context.categories.Add(new Category{Name = "Vacaciones"});
                await _context.SaveChangesAsync();
            }
        }
    }
    
}