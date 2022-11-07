using MajaTours.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MajaTours.Data
{
    public class DataContext: IdentityDbContext<User>
    {
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<Category> ?categories {get;set;}
        public DbSet<Product> Products { get; set; }
        public DbSet<TemporalSale> TemporalSales { get; set; }
          protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // LAS COLUMNAS NAME ES UN INDICE UNICO
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(c => c.Name).IsUnique();     
        }
    }
}