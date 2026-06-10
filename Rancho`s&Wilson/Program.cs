
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rancho_s.core.Entities;
using Rancho_s.core.Interfaces;
using Rancho_s.Repository.Data;
using Rancho_s.Repository.Data.DataSeed;
using Rancho_s.Repository.Repositories;
using Rancho_s.Services.Services;
using System.Text;

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




            // ── ADD Microsoft Identity ────────────────────────────────────

            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                // Password rules
                // Keep them reasonable — not too strict for a restaurant app
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;  // relaxed
                options.Password.RequireNonAlphanumeric = false;  // relaxed

                // User rules
                options.User.RequireUniqueEmail = true;

                // Lockout — after 5 wrong attempts, lock for 5 minutes
                // Protects against brute force attacks
                options.Lockout.MaxFailedAccessAttempts = 4;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })

               .AddEntityFrameworkStores<Rancho_sDbContext>()
               // AddDefaultTokenProviders adds support for
               // password reset tokens, email confirmation tokens later
               .AddDefaultTokenProviders();




            // ── JWT Authentication ────────────────────────────────────────
            var jwtSecret = builder.Configuration["JWT:Secret"]!;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //do reverse engineering 3shan y3rf y3ml validate l token ELygay
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,    // Reject expired tokens
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                                                   Encoding.UTF8.GetBytes(jwtSecret))
                };
            });

            builder.Services.AddAuthorization();




            #region Swaager 
            // ── Swagger WITH JWT support ──────────────────────────────────
            // This adds the "Authorize" button in Swagger so you can test
            // protected endpoints without Postman
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ranchp`s API", Version = "v1" });

                // Add JWT auth to Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,   
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token here}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            #endregion

            // ── Dependency Injection — Register your layers ───────────
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddEndpointsApiExplorer();
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

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
