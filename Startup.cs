using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using WebSiteDocuments.Service;
using WebSiteDocuments.Models;
using WebSiteDocuments.Data;




namespace WebSiteDocuments
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<INotificationService, NotificationService>();

            services.AddSignalR();


            services.AddScoped<RoleManagementService>();
            // Other service configurations
            services.AddControllersWithViews();
            services.AddRazorPages();
            

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<WebDocumentDb>()
                    .AddDefaultTokenProviders();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManagementService roleManagementService)
        {
            // Krijimi i roleve dhe lidhja e përdoruesve me rolet
            roleManagementService.CreateRolesAndAssignToUserAsync().Wait();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

    


            app.UseEndpoints(endpoints =>
            {

                endpoints.MapHub<CommunityHub>("/communityHub");
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
