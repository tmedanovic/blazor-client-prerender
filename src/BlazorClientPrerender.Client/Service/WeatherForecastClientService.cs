using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorClientPrerender.Shared.Data;
using BlazorClientPrerender.Shared.Service;
using Microsoft.AspNetCore.Components;

namespace BlazorClientPrerender.Client.Service 
{
    public class WeatherForecastClientService : IWeatherForecastService
    {
        private readonly HttpClient httpClient;

        public WeatherForecastClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
			this.httpClient.BaseAddress = new Uri("https://localhost:7001/api/");
			this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));   
			this.httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
			this.httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Methods", "*");
			this.httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "*");
			this.httpClient.DefaultRequestHeaders.Add("Access-Control-Max-Age", "86400");  
        }

        public async Task<WeatherForecast[]> GetForecastAsync()
        {
           return await this.httpClient.GetJsonAsync<WeatherForecast[]>("forecasts");
        }
    }
}