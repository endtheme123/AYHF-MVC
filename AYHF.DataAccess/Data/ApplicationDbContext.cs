
using AYHF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AYHF.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 4, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Action", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Action", DisplayOrder = 3 }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product { 
                    Id = 1,
                    Name="potato",
                    Description="fresh and yummy", 
                    Price=5.0, 
                    Stock=100, 
                    Ordered = 0,
                    ImageUrl = ""
                },
                new Product { 
                    Id = 2, 
                    Name = "tomato",
                    Description = "fresh and yummy",
                    Price = 5.0,
                    Stock = 100, 
                    Ordered = 0,
                    ImageUrl = ""
                },
                new Product {
                    Id = 3,
                    Name = "healthy burger",
                    Description = "tasty and yummy",
                    Price = 5.0,
                    Stock = 100,
                    Ordered = 0,
                    ImageUrl = ""
                },
                new Product { 
                    Id = 4, 
                    Name = "carrot",
                    Description = "fresh and yummy",
                    Price = 5.0,
                    Stock = 100,
                    Ordered =0,
                    ImageUrl = ""
                },
                new Product {
                    Id = 5, 
                    Name = "onion",
                    Description = "fresh and yummy",
                    Price = 5.0,
                    Stock = 100,
                    Ordered = 0,
                    ImageUrl = ""
                }
                );
        }
    }
}
