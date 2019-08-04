using ProductDataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using DBModel = ProductRepository.Models;

namespace ProductServiceTests
{
    public static class ProductTestData
    {
        public static Guid ProductId = Guid.Parse("72f4053d-b617-4111-9a41-f1f7d2383660");

        public static String ProductName = "Product Name";

        public static String ProductDescription = "Product Description";

        public static Guid CategoryId = Guid.Parse("4d7dea37-ee9b-4009-9f1a-0818d2684924");

        public static String CategoryName = "Category Name";

        public static List<Product> ProductList = new List<Product>
            {
                new Product { Id = ProductId, Name = ProductName, Description = ProductDescription,
                                Category = new Category { Id = CategoryId, Name = CategoryName } }
            };

        public static List<Product> EmptyProductList = null;

        public static List<DBModel.Category> DBCategoryList = new List<DBModel.Category>
            {
                new DBModel.Category { Id = CategoryId, Name = CategoryName }
            };
        
        public static List<DBModel.Product> DBProductList = new List<DBModel.Product>
            {
                new DBModel.Product { Id = ProductId, Name = ProductName, Description = ProductDescription,
                                CategoryId = CategoryId }
            };

    }
}
