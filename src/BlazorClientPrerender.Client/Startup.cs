using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using BlazorClientPrerender.Shared.Service;
using System.Net.Http;
using BlazorClientPrerender.Shared.Extensions;

namespace BlazorClientPrerender.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ApiBaseUrl>(s => new ApiBaseUrl("https://localhost:7001/api/"));
            services.AddSingleton<IWeatherForecastService, WeatherForecastClientService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
