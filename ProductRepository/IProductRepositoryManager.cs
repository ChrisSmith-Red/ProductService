using ProductDataTransferObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductRepository
{
    public interface IProductRepositoryManager
    {
        /// <summary>
        /// Creates the product.
        /// </summary>
        /// <param name="productName">Name of the product.</param>
        /// <param name="productDescription">The product description.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Guid> CreateProduct(String productName, String productDescription, String categoryName, CancellationToken cancellationToken);
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the products for category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductsForCategory(Guid categoryId, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Boolean> DeleteProduct(Guid productId, CancellationToken cancellationToken);
    }
}
