﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OrdersApp.Models.Contexts;
using OrdersApp.Models.Entities;
using OrdersApp.Models.Repositories;

namespace OrdersApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            string constr = Configuration.GetConnectionString("DefaultConnection") ?? "";


            services.AddTransient<IRepository<Provider>, ProvidersRepository>();
            services.AddTransient<IRepository<Order>, OrdersRepository>();
			services.AddTransient<IRepository<OrderItem>, OrderItemsRepository>();

			services.AddControllersWithViews();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(constr));

            services.AddDistributedMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
