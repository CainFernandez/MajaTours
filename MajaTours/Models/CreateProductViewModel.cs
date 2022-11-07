using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MajaTours.Models
{
    public class CreateProductViewModel : EditProductViewModel
    {
        [Required(ErrorMessage="Debe colocar la url de la imagen")]
        [Display (Name="Imagen")]
        [Column("image")]
        public string Image{ get; set; }

    }
}