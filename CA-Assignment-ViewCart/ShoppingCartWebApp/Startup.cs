using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            
            // add our database context into DI container
            services.AddDbContext<DBContext>(opt =>
                opt.UseLazyLoadingProxies().UseSqlServer(
                    Configuration.GetConnectionString("db_conn"))
            );

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
             [FromServices] DBContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            DB db = new DB(dbContext);
            

            if (!dbContext.Database.CanConnect())
            {
                dbContext.Database.EnsureCreated();
                db.Seed();
            }
            else {

                //List<Product> products = db.GetProductsList();
                //Console.WriteLine("123");

            }
            
            User user = dbContext.Users.FirstOrDefault(
                    x => x.Username == "jean"
                    );

            Product product = dbContext.products.FirstOrDefault(
                x => x.ProductName == ".NET ML"
                );


            User user1 = dbContext.Users.FirstOrDefault(
                x => x.Username == "jean"
                );

            Product product1 = dbContext.products.FirstOrDefault(
                x => x.ProductName == ".NET Charts"
                );

            User user2 = dbContext.Users.FirstOrDefault(
                x => x.Username == "jean"
                );

            Product product2 = dbContext.products.FirstOrDefault(
                x => x.ProductName == ".NET Charts"
                );

            User user3 = dbContext.Users.FirstOrDefault(
                x => x.Username == "jean"
                );

            Product product3 = dbContext.products.FirstOrDefault(
                x => x.ProductName == ".NET Charts"
                );

            db.AddLibraryToCart(user.Id, product.Id);
            db.AddLibraryToCart(user1.Id, product1.Id);
            db.AddLibraryToCart(user2.Id, product2.Id);
            db.AddLibraryToCart(user3.Id, product3.Id);
            


        }
    }
}
