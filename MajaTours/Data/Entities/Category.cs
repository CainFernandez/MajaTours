using System.ComponentModel.DataAnnotations;

namespace MajaTours.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        
        [Display(Name = "Categorìa")]
        [MaxLength(50, ErrorMessage = "El Campo {0} debe tener Máximo {1} caractéres.")]
        [Required (ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Name { get; set; }

    }
}