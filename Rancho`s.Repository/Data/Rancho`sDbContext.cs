using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rancho_s.core.Entities;

namespace Rancho_s.Repository.Data
{
    public class Rancho_sDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public Rancho_sDbContext(DbContextOptions<Rancho_sDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // this will call all identity tables
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Rancho_sDbContext).Assembly);

            modelBuilder.Entity<AppRole>().HasData(
    new AppRole { Id = 1, Name = "Admin", NormalizedName = "ADMIN", Description = "Restaurant administrator" },
    new AppRole { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER", Description = "Regular customer" },
    new AppRole { Id = 3, Name = "KitchenStaff", NormalizedName = "KITCHENSTAFF", Description = "Kitchen team member" }
);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
