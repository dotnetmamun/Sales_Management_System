using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Project_MVC_Core.Model
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }
        [Required, StringLength(50)]
        public string FoodItemName { get; set; } = default!;
        [Required, Column(TypeName = "money")]
        public decimal? Price { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }
        [Required, StringLength(40)]
        public string Picture { get; set; } = default!;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
    public class Order
    {
        public int OrderId { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? OrderDate { get; set; }
        [Required]
        public int? Quantity { get; set; }
        [Required, ForeignKey("FoodItem")]
        public int FoodItemId { get; set; }
        public virtual FoodItem? FoodItem { get; set; }
    }
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options) { }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodItem>().HasData(
               new FoodItem { FoodItemId = 1, FoodItemName = "Burger", Price = 150, ExpireDate = DateTime.Parse("2024-2-28"), Picture = "1.jpeg" },
             new FoodItem { FoodItemId = 2, FoodItemName = "Pizza", Price = 1150, ExpireDate = DateTime.Parse("2024-2-28"), Picture = "2.jpeg" }
               );
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, OrderDate = DateTime.Parse("2024-2-15"), Quantity = 10, FoodItemId = 1, },
                new Order { OrderId = 2, OrderDate = DateTime.Parse("2024-2-16"), Quantity = 5, FoodItemId = 2, }
                );
        }
    }
    
}