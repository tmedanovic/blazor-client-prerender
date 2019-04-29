using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorClientPrerender.Shared.Data;

namespace BlazorClientPrerender.Shared.Service
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync();
    }
}
