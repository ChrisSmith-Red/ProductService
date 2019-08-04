using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductRepository.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SeedData = ProductRepository.SeedData;

namespace ProductRepository.DbContext
{
    public class ProductRepositoryDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Initializes a new instance of the<see cref="ProductRepositoryDbContext"/> class.
        /// </summary>
        public ProductRepositoryDbContext()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepositoryDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ProductRepositoryDbContext(DbContextOptions<ProductRepositoryDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public DbSet<Category> Category { get; set; }

        /// <summary>
        /// Called when [model creating].
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Get the product seed data
            var products = GetProductSeedData();

            if (products != null && products.Count > 0)
            {                
                // Get a distinct list of categories from the data
                List<String> categories = products.Select(p => p.Category).Distinct().ToList();

                if (categories != null && categories.Count > 0)
                {
                    // Seed the category table and store the Ids
                    Dictionary<String, Guid> categoryIds = new Dictionary<String, Guid>();
                    foreach (var cat in categories)
                    {
                        Guid catId = Guid.NewGuid();
                        categoryIds.Add(cat, catId);  
                        builder.Entity<Category>().HasData(                
                            new Category { Id = catId, Name = cat}
                        );
                    }

                    // Now seed the product table
                    foreach (var product in products)
                    {
                        Guid catId = categoryIds.GetValueOrDefault(product.Category);
                         builder.Entity<Product>().HasData(                
                                new Product { Id = Guid.NewGuid(), Name = product.Name, 
                                        Description = product.Description, 
                                        CategoryId = catId});
                    }    
                }
            }
        }

        /// <summary>
        /// Gets the product seed data.
        /// </summary>
        /// <returns>List&lt;SeedData.Product&gt;.</returns>
        private List<SeedData.Product> GetProductSeedData()
        {
            // Read the data from the file
            var products = new List<SeedData.Product>();
            String path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SeedData\\product.json");
            using (StreamReader r = new StreamReader(path))
            {
                String json = r.ReadToEnd();
                if (!String.IsNullOrEmpty(json))
                {                                        
                    products = JsonConvert.DeserializeObject<List<SeedData.Product>>(json);
                }
            }

            return products;
        }
    }
}
