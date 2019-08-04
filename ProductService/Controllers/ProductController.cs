using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductDataTransferObjects;
using ProductRepository;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// The product repository manger
        /// </summary>
        private readonly IProductRepositoryManager ProductRepositoryManger;

        private readonly ILogger<ProductController> Logger;

        #region Public Methods

        #region public ProductController(IProductRepositoryManager productRepositoryManger)
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productRepositoryManger">The product repository manger.</param>
        public ProductController(IProductRepositoryManager productRepositoryManger, ILogger<ProductController> logger)
        {
            this.ProductRepositoryManger = productRepositoryManger;
            this.Logger = logger;
        }
        #endregion
        
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 400)]
        public async Task<IActionResult> Post([FromQuery] String productName, [FromQuery] String productDescrption, [FromQuery] String categoryName, CancellationToken cancellationToken)
        {        
            // Validate the request
            if (!IsValidRequest(productName) ||
                !IsValidRequest(productDescrption) ||
                !IsValidRequest(categoryName))
            {
                this.Logger.LogInformation("Invalid Request");
                return BadRequest();
            }  

            Guid productId = await this.ProductRepositoryManger.CreateProduct(productName, productDescrption, categoryName, cancellationToken);
            
            return this.Ok(productId);
        }

        [HttpGet]
        [Route("GetCategories")]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 204)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {       
            IEnumerable<Category> categoryList = await this.ProductRepositoryManger.GetCategories(cancellationToken);

            if (categoryList == null)
            {
                this.Logger.LogInformation("No Categories Found");
                return NoContent();
            }
                
            return this.Ok(categoryList);
        }

        [HttpGet]
        [Route("GetProducts")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 204)]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {       
            IEnumerable<Product> productList = await this.ProductRepositoryManger.GetProducts(cancellationToken);
            
            if (productList == null)
            {
                this.Logger.LogInformation("No Products Found");
                return NoContent();
            }

            return this.Ok(productList);
        }
        
        [HttpGet]
        [Route("GetProductsForCategory")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 204)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 400)]
        public async Task<IActionResult> GetProductsForCategory([FromQuery] Guid categoryId, CancellationToken cancellationToken)
        {
            // Validate the request
            if (!IsValidRequest(categoryId))
            {
                this.Logger.LogInformation("Invalid Request");
                return BadRequest();
            }  

            IEnumerable<Product> productList = await this.ProductRepositoryManger.GetProductsForCategory(categoryId, cancellationToken);
            
            if (productList == null)
            {
                this.Logger.LogInformation($"No Products Found for Category with Id  [{categoryId}]");
                return NoContent();
            }

            return this.Ok(productList);
        }

        [HttpDelete]
        [Route("{productId}/Delete")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 400)]
        public async Task<IActionResult> Delete([FromRoute] Guid productId, CancellationToken cancellationToken)
        {                    
            // Validate the request
            if (!IsValidRequest(productId))
            {
                this.Logger.LogInformation("Invalid Request");
                return BadRequest();
            }            
            
            Boolean productDeleted = await this.ProductRepositoryManger.DeleteProduct(productId, cancellationToken);
            
            return this.Ok(productDeleted);
        }

        #endregion

        #region Private Methods

        #region private void IsValidRequest(String requestValue)        
        /// <summary>
        /// Determines whether [is valid request] [the specified request value].
        /// </summary>
        /// <param name="requestValue">The request value.</param>
        /// <returns>
        ///   <c>true</c> if [is valid request] [the specified request value]; otherwise, <c>false</c>.
        /// </returns>
        private Boolean IsValidRequest(String requestValue)
        {
            Boolean result = true;

            // Validate we have a valid request value
            if (String.IsNullOrEmpty(requestValue))
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region private void IsValidRequest(Guid requestValue)        
        /// <summary>
        /// Determines whether [is valid request] [the specified request value].
        /// </summary>
        /// <param name="requestValue">The request value.</param>
        /// <returns>
        ///   <c>true</c> if [is valid request] [the specified request value]; otherwise, <c>false</c>.
        /// </returns>
        private Boolean IsValidRequest(Guid requestValue)
        {
            Boolean result = true;

            // Validate we have a valid request value
            if (requestValue == Guid.Empty ||
                requestValue == null)
            {
                result = false;
            }

            return result;
        }
        #endregion

        #endregion
    }
}
