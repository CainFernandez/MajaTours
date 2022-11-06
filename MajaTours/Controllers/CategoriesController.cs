

using MajaTours.Data;
using MajaTours.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MajaTours.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        
        //CONSTRUCTOR PARA INYECTAR A LA BASE DE DATOS.
            private readonly DataContext _context;
            public CategoriesController(DataContext context)
            {
                _context = context;

            }
        // FIN

        // CONTROLADOR PARA LA VISTA INDEX.
            public async Task<IActionResult> Index()
            {
                return View(await _context.categories.ToListAsync());
            }
        // FIN.

        // CONTROLADOR CREAR UNA NUEVA GATEGORIA.
            public IActionResult Create()
            {
                return View();
            }
            
            // INICIO DEL METODO POST: PARA ENVIAR DATOS A LA BASE DE DATOS Y VALIDAR LOS CAMPOS.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create( Category category)
                {
                    if (ModelState.IsValid) //Que cumpla con data anotetions
                    {
                        _context.Add(category);

                        //VALIDACION PARA NO PERMITIR REPETIR LA MISMA CATEGORIA. 
                            try
                            {
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                            catch (DbUpdateException dbUpdateException)
                            {
                                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                                {
                                    ModelState.AddModelError(string.Empty, "Ya existe una categoria con el mismo nombre.");
                                }
                                else{
                                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                                }
                            }
                            catch (Exception exception)
                            {
                                ModelState.AddModelError(string.Empty, exception.Message);
                            }
                        // FIN DE VALIDACION DE CATEGORIA.
                    }
                    return View(category);
                }
            // FIN DEL METODO POST PARA CATEGORIA
        // FIN DE CREAR UNA GATEGORIA NUEVA
        
        // CONTROLADOR EDITAR UNA CATEGORIA
            // INICIO DEL METODO GET: PARA EDITAR LA CATEGORÍA.
                public async Task<IActionResult> Edit(int? id) // El id es un parametro
                {
                    if (id == null || _context.categories == null)
                    {
                        return NotFound();
                    }
                    
                    var category = await _context.categories.FindAsync(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    return View(category);
                }
            // FIN
            
            // INICIO DEL METODO POST: PARA ENVIAR DATOS A LA BASE DE DATOS Y VALIDAR LOS CAMPOS.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id,Category category)
                {
                    if (id != category.Id)
                    {
                        return NotFound();
                    }
                    if (ModelState.IsValid) // si el modelo es valido.
                    {
                        //VALIDACION PARA NO REPETIR LA MISMA CATEGORIA. 
                        try
                        {
                            _context.Update(category);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        catch (DbUpdateException dbUpdateException)
                        {
                            if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                            {
                                ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                            }
                        }
                        catch (Exception exception)
                        {
                            ModelState.AddModelError(string.Empty, exception.Message);
                        }
                    }
                    return View(category);
                }
            // FIN DEL METODO POST PARA EDITAR LA CATEGORIA
        // FIN DE CONTROLARDOR PARA EDITAR UNA CATEGORIA.

        // CONTROLADOR PARA MOSTRAR UN DETALLE PARA CATEGORIA.
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null || _context.categories == null)
                {
                    return NotFound();
                }
                var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
        // FIN DEL METODO GET, DEL DETALLE

       
        // CONTROLADOR PARA ELMINAR UNA CATEGORÍA

            // METODO GET PARA MOSTRAR UNA CATEGORIA
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null || _context.categories == null)
                {
                    return NotFound();
                }
                var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }

             // INICIO DEL METODO POST : PARA ELMINAR POR COMPLETO LOS DATOS DE UNA CATEGORÍA
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                if (_context.categories == null)
                {
                    return Problem("Entity set 'DataContext.Country'  is null.");
                }
                var category = await _context.categories.FindAsync(id);
                if (category != null)
                {
                    _context.categories.Remove(category);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        
        // FIN DEL CONTROLADOR PARA ELIMINAR UNA CATEGORIA.  
    }
}