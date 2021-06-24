using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace Dev.Web.HostCreator
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
            //https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-compilation?view=aspnetcore-5.0&tabs=visual-studio
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddControllersWithViews();
        }


        private void ConfigureApplicationParts(ApplicationPartManager apm)
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var assemblyFiles = Directory.GetFiles(rootPath, "*.dll");
            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFile(assemblyFile);
                    if (assemblyFile.EndsWith(this.GetType().Namespace + ".Views.dll") || assemblyFile.EndsWith(this.GetType().Namespace + ".dll"))
                        continue;
                    else if (assemblyFile.EndsWith(".Views.dll"))
                        apm.ApplicationParts.Add(new CompiledRazorAssemblyPart(assembly));
                    else
                        apm.ApplicationParts.Add(new AssemblyPart(assembly));
                }
                catch (Exception) { }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
