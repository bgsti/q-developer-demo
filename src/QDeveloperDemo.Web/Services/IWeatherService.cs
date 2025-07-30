using QDeveloperDemo.Web.Models;

namespace QDeveloperDemo.Web.Services;

public interface IWeatherService
{
    Task<IEnumerable<WeatherForecast>> GetForecastAsync(int days = 5);
    Task<WeatherForecast> GetCurrentWeatherAsync();
}