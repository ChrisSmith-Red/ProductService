using Microsoft.EntityFrameworkCore;
using ProductRepository.DbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductRepository
{
    public class DatabaseSeeding
    {
        public static void InitialiseDatabase(ProductRepositoryDbContext context)
        {
            // Perform any migrations and save the changes
            context.Database.Migrate();

            context.SaveChanges();
        }
    }
}