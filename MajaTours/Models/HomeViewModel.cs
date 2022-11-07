using MajaTours.Data.Entities;
namespace MajaTours.Models
{
    public class HomeViewModel
    {
        public ICollection<Product> Products { get; set; }
        public float Quantity { get; set; }
    }
}