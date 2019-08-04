using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductRepository;
using ProductRepository.DbContext;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DTOs = ProductDataTransferObjects;


namespace ProductServiceTests
{
    [TestClass]
    public class ProductRepositoryManagerTests
    { 
        private Mock<Func<ProductRepositoryDbContext>> ProductRepositoryDbContextResolver;
        private ProductRepositoryDbContext ProductRepositoryDbContext;
        private Func<DbContextOptions<ProductRepositoryDbContext>, ProductRepositoryDbContext> ContextResolver = (options) => { return new ProductRepositoryDbContext(options); };
        private Func<ProductRepositoryDbContext> Resolver;

        public ProductRepositoryManagerTests()
        {            
            ProductRepositoryDbContextResolver = new Mock<Func<ProductRepositoryDbContext>>();

            var builder = new DbContextOptionsBuilder<ProductRepositoryDbContext>().UseInMemoryDatabase($"Product-{Guid.NewGuid()}");
            ProductRepositoryDbContext = ContextResolver(builder.Options);

            // Add data to the in memory db
            ProductRepositoryDbContext.Category.AddRange(ProductTestData.DBCategoryList);
            ProductRepositoryDbContext.Product.AddRange(ProductTestData.DBProductList);

            ProductRepositoryDbContext.SaveChanges();

            Resolver = () => { return new ProductRepositoryDbContext(builder.Options); };
        }

        #region Constructor Tests
        [TestMethod]
        public void ProductRepositoryManager_CanBeCreated_IsCreated()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            productRepositoryManager.ShouldNotBeNull();
        }
        #endregion

        #region Create Product Tests
        [TestMethod]
        public void ProductRepositoryManager_CreateProductForExistingCategory_ProductsReturned()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            String productName = "Product Test Name";
            String productDescription = "Product Test Name";

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.CreateProduct(productName, productDescription, ProductTestData.CategoryName, cancellationToken);
                result.ShouldNotBeNull();

                // Retrieve the newly created product
                var products = await productRepositoryManager.GetProducts(cancellationToken);
                var product = products.Where(p => p.Id == result).SingleOrDefault();
                product.ShouldNotBeNull();
                product.Name.ShouldBe(productName);
                product.Description.ShouldBe(productDescription);
            });
        }

        [TestMethod]
        public void ProductRepositoryManager_CreateProductForNewCategory_InvalidProduct()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            String productName = "Product Test Name";
            String productDescription = "Product Test Name";
            String categoryName = "Product Test Name";

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.CreateProduct(productName, productDescription, categoryName, cancellationToken);
                result.ShouldNotBeNull();

                // Retrieve the newly created product
                var products = await productRepositoryManager.GetProducts(cancellationToken);
                var product = products.Where(p => p.Id == result).SingleOrDefault();
                product.ShouldNotBeNull();
                product.Name.ShouldBe(productName);
                product.Description.ShouldBe(productDescription);
                product.Category.ShouldNotBeNull();
                product.Category.Name.ShouldBe(categoryName);
            });
        }

        [TestMethod]
        public void ProductRepositoryManager_CreateDuplicateProduct_InvalidProduct()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.CreateProduct(ProductTestData.ProductName, ProductTestData.ProductDescription, ProductTestData.CategoryName, cancellationToken);
                result.ShouldBe(Guid.Empty);
            });
        }
        #endregion

        #region Get Products Tests
        [TestMethod]
        public void ProductRepositoryManager_GetProducts_ProductsReturned()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.GetProducts(cancellationToken);
                result.ToList().ShouldNotBeNull();
                result.ToList().Count().ShouldBeGreaterThan(0);
                var product = result.ToList()[0];
                product.Id.ShouldBe(ProductTestData.ProductId);
                product.Name.ShouldBe(ProductTestData.ProductName);
                product.Description.ShouldBe(ProductTestData.ProductDescription);
                product.Category.ShouldNotBeNull();
                product.Category.Id.ShouldBe(ProductTestData.CategoryId);
                product.Category.Name.ShouldBe(ProductTestData.CategoryName);
            });
        }

        [TestMethod]
        public void ProductRepositoryManager_GetProducts_NoProductsReturned()
        {
            // Create an empty db
            ProductRepositoryDbContextResolver = new Mock<Func<ProductRepositoryDbContext>>();
            var builder = new DbContextOptionsBuilder<ProductRepositoryDbContext>().UseInMemoryDatabase($"Product-{Guid.NewGuid()}");
            ProductRepositoryDbContext = ContextResolver(builder.Options);
            ProductRepositoryDbContext.SaveChanges();
            Resolver = () => { return new ProductRepositoryDbContext(builder.Options); };

            var productRepositoryManager = new ProductRepositoryManager(Resolver);
            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.GetProducts(cancellationToken);
                result.ShouldBeNull();
            });
        }
        #endregion

        #region Get Categories Tests
        [TestMethod]
        public void ProductRepositoryManager_GetCategories_ProductsReturned()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.GetCategories(cancellationToken);
                result.ToList().ShouldNotBeNull();
                result.ToList().Count().ShouldBeGreaterThan(0);
                var category = result.ToList()[0];
                category.Id.ShouldBe(ProductTestData.CategoryId);
                category.Name.ShouldBe(ProductTestData.CategoryName);
            });
        }

        [TestMethod]
        public void ProductRepositoryManager_GetCategories_NoProductsReturned()
        {
            // Create an empty db
            ProductRepositoryDbContextResolver = new Mock<Func<ProductRepositoryDbContext>>();
            var builder = new DbContextOptionsBuilder<ProductRepositoryDbContext>().UseInMemoryDatabase($"Product-{Guid.NewGuid()}");
            ProductRepositoryDbContext = ContextResolver(builder.Options);
            ProductRepositoryDbContext.SaveChanges();
            Resolver = () => { return new ProductRepositoryDbContext(builder.Options); };

            var productRepositoryManager = new ProductRepositoryManager(Resolver);
            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.GetCategories(cancellationToken);
                result.ShouldBeNull();
            });
        }
        #endregion

        #region Get Product For Category Tests
        [TestMethod]
        public void ProductRepositoryManager_GetProductForCategory_ProductsReturned()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.GetProductsForCategory(ProductTestData.CategoryId, cancellationToken);
                result.ToList().ShouldNotBeNull();
                result.ToList().Count().ShouldBeGreaterThan(0);
                var product = result.ToList()[0];
                product.Id.ShouldBe(ProductTestData.ProductId);
                product.Name.ShouldBe(ProductTestData.ProductName);
                product.Description.ShouldBe(ProductTestData.ProductDescription);
                product.Category.ShouldNotBeNull();
                product.Category.Id.ShouldBe(ProductTestData.CategoryId);
                product.Category.Name.ShouldBe(ProductTestData.CategoryName);
            });
        }

        [TestMethod]
        public void ProductRepositoryManager_GetProductForCategory_NoProductsReturned()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.GetProductsForCategory(Guid.NewGuid(), cancellationToken);
                result.ShouldBeNull();
            });
        }
        #endregion

        #region Delete Product Tests
        [TestMethod]
        public void ProductRepositoryManager_DeleteProduct_ProductsReturned()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.DeleteProduct(ProductTestData.ProductId, cancellationToken);
                result.ShouldBeTrue();
            });
        }

        [TestMethod]
        public void ProductRepositoryManager_DeleteProduct_InvalidProduct()
        {
            var productRepositoryManager = new ProductRepositoryManager(Resolver);

            CancellationToken cancellationToken = new CancellationToken();

            Should.NotThrow(async () =>
            {
                var result = await productRepositoryManager.DeleteProduct(Guid.NewGuid(), cancellationToken);
                result.ShouldBeFalse();
            });
        }
        #endregion
    }
}
