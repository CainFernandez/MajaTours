using MajaTours.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MajaTours.Data
{
    public class DataContext: DbContext
    {
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<Category> ?categories {get;set;}
          protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // LAS COLUMNAS NAME ES UN INDICE UNICO
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
                  
        }
    }
}