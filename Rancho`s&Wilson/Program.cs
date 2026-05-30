
using Microsoft.EntityFrameworkCore;
using Rancho_s.core.Interfaces;
using Rancho_s.Repository.Data;
using Rancho_s.Repository.Data.DataSeed;
using Rancho_s.Repository.Repositories;
using Rancho_s.Services.Services;

namespace Rancho_s_Wilson
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure services Add services to the container

            builder.Services.AddControllers();

            // ── Database ──────────────────────────────────────────────
            builder.Services.AddDbContext<Rancho_sDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ── Dependency Injection — Register your layers ───────────
            // Every time someone asks for IProductRepository, give them ProductRepository
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ProductService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            var app = builder.Build();

            #region Ubdate DataBase

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbcontext = services.GetRequiredService<Rancho_sDbContext>();//like in ctor of
                await dbcontext.Database.MigrateAsync();

                await RanchosSeed.SeedAsync(dbcontext); // call seeding method to seed data in database
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while migrating the database.");// kda  log error in console  lw fe moshkla fe migration
            }



            #endregion

          
            #region Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
