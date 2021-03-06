using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorClientPrerender.Service;
using BlazorClientPrerender.Shared.Service;
using System.Net.Http;
using BlazorClientPrerender.Shared.Extensions;

namespace BlazorClientPrerender.Startup
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; set; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProxy();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Register class holding our base REST API address, it feels more natural to set base URL
            // inside service than using typed http clients or factories, even more so when we are using
            // sam services on client side also
            services.AddSingleton<ApiBaseUrl>(s => new ApiBaseUrl("https://localhost:7001/api/"));

            // Register HttpClient, in dev mode we won't validate SSL certs because they are self signed
            services.AddScoped<HttpClient>(s => 
            {
                return Environment.IsDevelopment() ? 
                    new HttpClient(new HttpClientHandler()
                    {
                          ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }) : new HttpClient();
            });
            
            // Register custom services
            services.AddScoped<IWeatherForecastService, WeatherForecastClientService>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Route _framework requests to client side generated .dlls 
            // ( Our client side blazor is running on localhost:5000 )
            app.MapWhen((context) => context.Request.Path.Value.StartsWith("/_framework"), 
                builder => builder.RunProxy(new ProxyOptions()
            {
                Scheme = "http",
                Host = new HostString("localhost", 5000)
            }));

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
