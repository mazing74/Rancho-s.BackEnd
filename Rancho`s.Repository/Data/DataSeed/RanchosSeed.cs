using Rancho_s.core.Entities;
using System.Text.Json;

namespace Rancho_s.Repository.Data.DataSeed
{
    public static class RanchosSeed
    {
        // call from Program.cs
        public static async Task SeedAsync(Rancho_sDbContext _context)
        {
            if (_context.Categories.Any() == false)
            {
                //Seeding Categories 
                var categoriesData = File.ReadAllText("../Rancho`s.Repository/Data/DataSeed/categories.json");// read json file and do cerylizion
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData); // do deserialization
                if (categories?.Count > 0)
                {  // if there is no data in json file return
                    foreach (var category in categories)
                        await _context.Set<Category>().AddAsync(category);

                    await _context.SaveChangesAsync();
                }

            }

            if (_context.Products.Any() == false)
            {            
                //Seeding Products

                var productsData = File.ReadAllText("../Rancho`s.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                        await _context.Set<Product>().AddAsync(product);

                    await _context.SaveChangesAsync();
                }
            }


        }
    }
}
