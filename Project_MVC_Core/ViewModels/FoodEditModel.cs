using Project_MVC_Core.Model;
using System.ComponentModel.DataAnnotations;

namespace Project_MVC_Core.ViewModels
{
    public class FoodEditModel
    {
        public int FoodItemId { get; set; }
        [Required, StringLength(50)]
        public string FoodItemName { get; set; } = default!;
        [Required, DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }
        public IFormFile Picture { get; set; } = default!;
        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
