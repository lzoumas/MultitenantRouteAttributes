using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MultitenantRouteAttributes
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add("/Sites/{2}/Views/Default/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Sites/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "Website1",
                    template: "{controller=Default}/{action=Index}",
                    defaults: new { area = "Website1" },
                    constraints: new { _ = new DomainConstraint("localhost") });

                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Default}/{action=Index}",
                    defaults: new { area = "_default" },
                    constraints: new { _ = new DomainConstraint("dumdumhead") });

            });
        }

        public class DomainConstraint : IRouteConstraint
        {
            private readonly string[] domains;

            public DomainConstraint(params string[] domains)
            {
                this.domains = domains ?? throw new ArgumentNullException(nameof(domains));
            }

            public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
            {
                string domain =
#if DEBUG
                    // A domain specified as a query parameter takes precedence 
                    // over the hostname (in debug compile only).
                    // This allows for testing without configuring IIS with a 
                    // static IP or editing the local hosts file.
                    httpContext.Request.Query["domain"];
#else
                null;
#endif
                if (string.IsNullOrEmpty(domain))
                    domain = httpContext.Request.Host.Host;

                return domains.Contains(domain);
            }
        }
    }
}
