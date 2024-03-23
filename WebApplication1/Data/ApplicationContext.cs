using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property("Price").HasPrecision(18, 2);

            Product[] productList =
            {
                new() {Id = 1, Title = "Apple", Description = "Red Apple", Price = 2000},
                new() {Id = 2, Title = "Orange Juice", Description = "Orange Juice", Price = 4000},
                new() {Id = 3, Title = "Ice Cream", Description = "Delicious Ice Cream", Price = 2000}
            };

            modelBuilder.Entity<Product>().HasData(productList);
        }
    }
}
