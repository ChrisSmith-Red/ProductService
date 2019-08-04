using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductRepository;
using ProductRepository.DbContext;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Startup.Configuration = builder.Build();
            Startup.HostingEnvironment = env;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Gets or sets the hosting environment.
        /// </summary>
        /// <value>
        /// The hosting environment.
        /// </value>
        public static IHostingEnvironment HostingEnvironment { get; set; }

        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Product Service", Version = "v1" });
            });

            services.AddSingleton(Startup.Configuration);
            services.AddSingleton<IProductRepositoryManager, ProductRepositoryManager>();
            services.AddTransient<Func<ProductRepositoryDbContext>>(cont => () => cont.GetService<ProductRepositoryDbContext>());

            // Db configuration
            var connectionString = Configuration.GetConnectionString("ProductRepositoryDbContext");
            String migrationsAssembly = typeof(ProductRepositoryDbContext).GetTypeInfo().Assembly.GetName().Name;
            
            services.AddDbContext<ProductRepositoryDbContext>(builder =>
                    builder.UseMySql(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                    .AddTransient<ProductRepositoryDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            this.InitialiseDatabase(app, env).Wait();

            app.UseMvc();  
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Service"); });
        }

        private async Task InitialiseDatabase(IApplicationBuilder app, IHostingEnvironment environment)
        {
            using (IServiceScope scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                ProductRepositoryDbContext productRepositoryDbContext = scope.ServiceProvider.GetRequiredService<ProductRepositoryDbContext>();

                DatabaseSeeding.InitialiseDatabase(productRepositoryDbContext);
            }
        }
    }
}
