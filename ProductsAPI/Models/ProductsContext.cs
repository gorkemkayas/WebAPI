using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Models
{
    public class ProductsContext : IdentityDbContext<AppUser,AppRole, int>
    {
        public ProductsContext(DbContextOptions<ProductsContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Iphone 14" , Price = 6000, IsActive = true },
                new Product { ProductId = 2, ProductName = "Iphone 15" , Price = 7000, IsActive = true },
                new Product { ProductId = 3, ProductName = "Iphone 15 Pro" , Price = 8000, IsActive = true },
                new Product { ProductId = 4, ProductName = "Iphone 15 Pro Max" , Price = 9000, IsActive = true });
        }
        public DbSet<Product> Products { get; set; }
    }
}