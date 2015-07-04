using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using EmployeeApp.Models;
using Microsoft.AspNet.Http;

namespace EmployeeApp
{
    public class Startup
    { 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //  services.AddEntityFramework()
            //      .AddDbContext<EmployeeContext>(options => options.UseInMemoryStore());

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorPage();
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();	
        }  
    }
}