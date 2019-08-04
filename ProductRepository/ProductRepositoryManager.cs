using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductRepository.DbContext;
using ProductRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DTOs = ProductDataTransferObjects;

namespace ProductRepository
{
    public class ProductRepositoryManager : IProductRepositoryManager
    {
        private readonly Func<ProductRepositoryDbContext> ProductRepositoryDbContext;

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepositoryManager"/> class.
        /// </summary>
        /// <param name="productRepositoryDbContext">The product repository database context.</param>
        public ProductRepositoryManager(Func<ProductRepositoryDbContext> productRepositoryDbContext)
        {
            this.ProductRepositoryDbContext = productRepositoryDbContext;
        }
        #endregion

        #region public async Task<Guid> CreateProduct(String productName, String productDescription, String categoryName, CancellationToken cancellationToken)
//
        public async Task<Guid> CreateProduct(String productName, String productDescription, String categoryName, CancellationToken cancellationToken)
        {
            Guid productId = Guid.Empty;

            if (! await IsExistingProduct(productName, categoryName, cancellationToken))
            {
                productId = Guid.NewGuid();

                // Get the category
                Guid categoryId = await GetCategoryId(categoryName, cancellationToken);
                if (categoryId == Guid.Empty)
                {
                    // Create the category if it doesn't exist
                    categoryId = await CreateCategory(categoryName, cancellationToken);
                }
                      
                using (var context = this.ProductRepositoryDbContext())
                {
                    // Create the product record to be added
                    Product product = new Product
                    {                    
                        Id = productId,
                        Name = productName,
                        Description = productDescription,
                        CategoryId = categoryId
                    };

                    // Add and save the record
                    await context.Product.AddAsync(product, cancellationToken);                
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
               
            return productId;
        }
        #endregion

        #region public async Task<bool> DeleteProduct(Guid productId, CancellationToken cancellationToken)        
        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> DeleteProduct(Guid productId, CancellationToken cancellationToken)
        {       
            Boolean result = false;

            using (var context = this.ProductRepositoryDbContext())
            {
                // Get the record to be deleted
                var productRecord = await context.Product.Where(p => p.Id == productId).SingleOrDefaultAsync(cancellationToken);
                
                if (productRecord != null)
                {
                    // Delete the record if it exists
                    context.Remove(productRecord);
                    await context.SaveChangesAsync(cancellationToken);

                    result = true;
                }
            }

            return result;
        }
        #endregion

        #region public Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;IEnumerable&lt;DTOs.Category&gt;&gt;.</returns>
        public async Task<IEnumerable<DTOs.Category>> GetCategories(CancellationToken cancellationToken)
        {
            List<DTOs.Category> result = null;

            using (var context = this.ProductRepositoryDbContext())
            {
                // Get the list of categories from the database
                var categoryList = await context.Category.ToListAsync(cancellationToken);
                
                if (categoryList != null && categoryList.Count > 0)
                {
                    result = new List<DTOs.Category>();
                    // Convert each category into a response object and add to the return list
                    // TODO: This should really be done in a factory to avoid clutter in here!
                    foreach (var category in categoryList)
                    {
                        DTOs.Category c = new DTOs.Category
                        {
                            Id = category.Id,
                            Name = category.Name
                        };
                        
                        result.Add(c);
                    }
                }
            }

            return result;
        }
        #endregion

        #region public Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;IEnumerable&lt;DTOs.Product&gt;&gt;.</returns>
        public async Task<IEnumerable<DTOs.Product>> GetProducts(CancellationToken cancellationToken)
        {
            List<DTOs.Product> result = null;

            using (var context = this.ProductRepositoryDbContext())
            {
                // Get the list of products from the database
                var productList = await context.Product.Include(p => p.Category)
                                                        .ToListAsync(cancellationToken);
                
                if (productList != null && productList.Count > 0)
                {
                    result = new List<DTOs.Product>();
                    // Convert each product into a response object and add to the return list
                    // TODO: This should really be done in a factory to avoid clutter in here!
                    foreach (var product in productList)
                    {   
                        DTOs.Product p = new DTOs.Product
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            Category = new DTOs.Category { Id = product.Category.Id, Name = product.Category.Name }
                        };
                        
                        result.Add(p);
                    }
                }
            }

            return result;
        }
        #endregion

        #region public async Task<IEnumerable<Product>> GetProductsForCategory(Guid categoryId, CancellationToken cancellationToken)         
        /// <summary>
        /// Gets the products for category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;IEnumerable&lt;DTOs.Product&gt;&gt;.</returns>
        public async Task<IEnumerable<DTOs.Product>> GetProductsForCategory(Guid categoryId, CancellationToken cancellationToken)
        {
            List<DTOs.Product> result = null;

            using (var context = this.ProductRepositoryDbContext())
            {
                // Get the list of products from the database which match the search criteria
                var productList = await context.Product.Include(p => p.Category)
                                                       .Where(c => c.Category.Id == categoryId)
                                                       .ToListAsync(cancellationToken);

                if (productList != null && productList.Count > 0)
                {
                    result = new List<DTOs.Product>();
                    // Convert each product into a response object and add to the return list
                    // Note: This should really be done in a factory to avoid clutter in here!
                    foreach (var product in productList)
                    {
                        DTOs.Product p = new DTOs.Product
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            Category = new DTOs.Category { Id = product.Category.Id, Name = product.Category.Name }
                        };
                        
                        result.Add(p);
                    }
                }
            }

            return result;
        }
        #endregion

        #region private async Task<Guid> GetCategoryId(String categoryName, CancellationToken cancellationToken)
        /// <summary>
        /// Gets the category identifier.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;Guid&gt;.</returns>
        private async Task<Guid> GetCategoryId(String categoryName, CancellationToken cancellationToken)
        {
            // Retieve the category Id for the given name
            using (var context = this.ProductRepositoryDbContext())
            {
                var categoryRecord = await context.Category.Where(c => c.Name == categoryName).SingleOrDefaultAsync(cancellationToken);
                
                if (categoryRecord == null)
                {
                    return Guid.Empty;
                }
                
                return categoryRecord.Id;
            }
        }
        #endregion

        #region private async Task<Guid> CreateCategory(String categoryName, CancellationToken cancellationToken)
        /// <summary>
        /// Creates the category.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;Guid&gt;.</returns>
        private async Task<Guid> CreateCategory(String categoryName, CancellationToken cancellationToken)
        {
            Guid categoryId = Guid.NewGuid();

            using (var context = this.ProductRepositoryDbContext())
            {
                // Create the category to be added to the db
                Category category = new Category
                {                    
                    Id = categoryId,
                    Name = categoryName
                };

                // Add the category to the database and save the change
                await context.Category.AddAsync(category, cancellationToken);                
                await context.SaveChangesAsync(cancellationToken);
            }

            return categoryId;
        }
        #endregion    

        #region private async Task<Boolean> IsExistingProduct(String productName, CancellationToken cancellationToken)
        /// <summary>
        /// Determines whether [is existing product] [the specified product name].
        /// </summary>
        /// <param name="productName">Name of the product.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;Boolean&gt;.</returns>
        private async Task<Boolean> IsExistingProduct(String productName, String categoryName, CancellationToken cancellationToken)
        {
            Boolean result = false;

            using (var context = this.ProductRepositoryDbContext())
            {
                // Get the list of products from the database
                var productList = await context.Product.Include(p => p.Category)
                                                        .ToListAsync(cancellationToken);

                if (productList != null && productList.Count > 0)
                {
                    var product = productList.Where(p => p.Name == productName &&
                                                         p.Category.Name == categoryName).SingleOrDefault();

                    result = product != null;
                }
            }

            return result;
        }
        #endregion            
    }
}
