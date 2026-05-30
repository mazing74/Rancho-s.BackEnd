using Microsoft.EntityFrameworkCore;
using Rancho_s.core.Entities;
using Rancho_s.Repository.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Repository.Data
{
    public class Rancho_sDbContext : DbContext
    {
        public Rancho_sDbContext(DbContextOptions<Rancho_sDbContext> options) : base(options)
        {
             
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Rancho_sDbContext).Assembly);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
