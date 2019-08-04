using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductDataTransferObjects;
using ProductRepository;
using ProductService.Controllers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace ProductServiceTests
{
    [TestClass]
    public class ProductControllerTests
    {
        #region Constructor Tests
        [TestMethod]
        public void ProductController_CanBeCreated_IsCreated()
        {
            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);

            controller.ShouldNotBeNull();
        }
        #endregion

        #region POST Tests
        [TestMethod]
        public void ProductController_POST_OK()
        {
            Guid productId = Guid.NewGuid();

            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.CreateProduct(It.IsAny<String>(), It.IsAny<String>(),
                                                                  It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(productId);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);

            IActionResult postResult = null;

            Should.NotThrow(async () =>
            {
                postResult = await controller.Post(ProductTestData.ProductName, ProductTestData.ProductDescription,
                                                        ProductTestData.CategoryName, CancellationToken.None);
            });

            // Check the response
            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<OkObjectResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.OK);
            objectResult.Value.ShouldBe(productId);
        }

        [TestMethod]
        public void ProductController_POST_InvalidProductName_BadRequestReturned()
        {
            Guid productId = Guid.NewGuid();

            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.CreateProduct(It.IsAny<String>(), It.IsAny<String>(),
                                                                  It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(productId);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);

            IActionResult postResult = null;
           
            Should.NotThrow(async () =>
            {
                postResult = await controller.Post(String.Empty, ProductTestData.ProductDescription,
                                                        ProductTestData.CategoryName, CancellationToken.None);
            });

            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<BadRequestResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ProductController_POST_InvalidProductDescription_BadRequestReturned()
        {
            Guid productId = Guid.NewGuid();

            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.CreateProduct(It.IsAny<String>(), It.IsAny<String>(),
                                                                  It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(productId);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);

            IActionResult postResult = null;
                      
            Should.NotThrow(async () =>
            {
                postResult = await controller.Post(ProductTestData.ProductName, String.Empty,
                                                        ProductTestData.CategoryName, CancellationToken.None);
            });

            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<BadRequestResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ProductController_POST_InvalidCategoryName_BadRequestReturned()
        {
            Guid productId = Guid.NewGuid();

            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.CreateProduct(It.IsAny<String>(), It.IsAny<String>(),
                                                                  It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(productId);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);

            IActionResult postResult = null;
                     
            Should.NotThrow(async () =>
            {
                postResult = await controller.Post(ProductTestData.ProductName, ProductTestData.ProductDescription,
                                                        String.Empty, CancellationToken.None);
            });

            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<BadRequestResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.BadRequest);
        }
        #endregion

        #region DELETE Tests
        [TestMethod]
        public void ProductController_DELETE_OK()
        {
            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.DeleteProduct(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(true);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);

            IActionResult postResult = null;

            Should.NotThrow(async () =>
            {
                postResult = await controller.Delete(ProductTestData.ProductId, CancellationToken.None);
            });

            // Check the response
            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<OkObjectResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.OK);
            objectResult.Value.ShouldBe(true);
        }

        [TestMethod]
        public void ProductController_DELETE_InvalidProductName_BadRequestReturned()
        {
            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.DeleteProduct(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(true);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);

            IActionResult postResult = null;
                
            Should.NotThrow(async () =>
            {
                postResult = await controller.Delete(Guid.Empty, CancellationToken.None);
            });

            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<BadRequestResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.BadRequest);
        }
        #endregion
        
        #region GET Tests
        // TODO: Have only included one set of GET tests here...the other would be very similar.
        [TestMethod]
        public void ProductController_GET_ProductsReturned_OK()
        {
            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.GetProducts(It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(ProductTestData.ProductList);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);
            
            IActionResult postResult = null;

            Should.NotThrow(async () =>
            {
                postResult = await controller.GetProducts(CancellationToken.None);
            });

            // Check the response
            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<OkObjectResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.OK);
            var productList = objectResult.Value.ShouldBeOfType<List<Product>>();
            productList.ShouldNotBeNull();
            productList.Count.ShouldBe(1);
            var product = productList[0];
            product.Id.ShouldBe(ProductTestData.ProductId);
            product.Name.ShouldBe(ProductTestData.ProductName);
            product.Description.ShouldBe(ProductTestData.ProductDescription);
            product.Category.ShouldNotBeNull();
            product.Category.Id.ShouldBe(ProductTestData.CategoryId);
            product.Category.Name.ShouldBe(ProductTestData.CategoryName);
        }

        [TestMethod]
        public void ProductController_GET_NoProductsReturned()
        {
            Mock<IProductRepositoryManager> productRepositoryManager = new Mock<IProductRepositoryManager>();
            Mock<ILogger<ProductController>> logger = new Mock<ILogger<ProductController>>();
            productRepositoryManager.Setup(rm => rm.GetProducts(It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(ProductTestData.EmptyProductList);

            ProductController controller = new ProductController(productRepositoryManager.Object, logger.Object);
            
            IActionResult postResult = null;

            Should.NotThrow(async () =>
            {
                postResult = await controller.GetProducts(CancellationToken.None);
            });

            // Check the response
            postResult.ShouldNotBeNull();
            var objectResult = postResult.ShouldBeOfType<NoContentResult>();
            objectResult.StatusCode.ShouldBe((Int32)HttpStatusCode.NoContent);
        }        
        #endregion        
    }
}
