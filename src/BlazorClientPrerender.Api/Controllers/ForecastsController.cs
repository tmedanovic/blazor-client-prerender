using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorClientPrerender.Shared.Service;
using Microsoft.AspNetCore.Mvc;

namespace BlazorClientPrerender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastsController : ControllerBase
    {
        private readonly IWeatherForecastService weatherForecastService;
        
        public ForecastsController(IWeatherForecastService weatherForecastService)
        {
            this.weatherForecastService = weatherForecastService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var forecasts = await this.weatherForecastService.GetForecastAsync();
            return Ok(forecasts);
        }
    }
}
