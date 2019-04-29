using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorClientPrerender.Shared.Data;
using BlazorClientPrerender.Shared.Extensions;
using BlazorClientPrerender.Shared.Service;
using Microsoft.AspNetCore.Components;

namespace BlazorClientPrerender.Shared.Service 
{
    public class WeatherForecastClientService : IWeatherForecastService
    {
        private readonly HttpClient httpClient;

        public WeatherForecastClientService(HttpClient httpClient, ApiBaseUrl apiBaseUrl)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = apiBaseUrl.Uri;
        }

        public async Task<WeatherForecast[]> GetForecastAsync()
        {
           return await this.httpClient.GetJsonAsync<WeatherForecast[]>("forecasts");
        }
    }
}