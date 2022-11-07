using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MajaTours.Models
{
    public class EditProductViewModel
    {
        public int Id {get; set;}
        
        [Display(Name="Nombre")]
        [MaxLength(50, ErrorMessage ="El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public string Name {get; set;}

        [Display(Name ="Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description {get; set;}

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; } 

        [Required(ErrorMessage="Debe colocar la url de la imagen")]
        [Display (Name="Imagen")]
        [Column("image")]
        public string Image{ get; set; }
    }
}