using Microsoft.AspNetCore.Mvc;
using QDeveloperDemo.Web.Models;
using QDeveloperDemo.Web.Services;

namespace QDeveloperDemo.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        _logger.LogInformation("Getting weather forecast");
        return await _weatherService.GetForecastAsync();
    }

    [HttpGet("current")]
    public async Task<WeatherForecast> GetCurrent()
    {
        _logger.LogInformation("Getting current weather");
        return await _weatherService.GetCurrentWeatherAsync();
    }

    [HttpGet("{days:int}")]
    public async Task<IEnumerable<WeatherForecast>> GetForecast(int days)
    {
        if (days < 1 || days > 30)
        {
            throw new ArgumentOutOfRangeException(nameof(days), "Days must be between 1 and 30");
        }

        _logger.LogInformation("Getting weather forecast for {Days} days", days);
        return await _weatherService.GetForecastAsync(days);
    }
}