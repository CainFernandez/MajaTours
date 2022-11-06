using MajaTours.Data.Entities;

namespace MajaTours.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        //############### INYECCIÓN A LA BASE DE DATOS ###########
        public SeedDb(DataContext context)
        {
            _context = context;
        } 
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // crea la base datos
            await CheckCategoriesAsync();      
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