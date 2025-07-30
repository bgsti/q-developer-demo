using QDeveloperDemo.Web.Services;

namespace QDeveloperDemo.Tests.Services;

public class WeatherServiceTests
{
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        _weatherService = new WeatherService();
    }

    [Fact]
    public async Task GetForecastAsync_ReturnsCorrectNumberOfDays()
    {
        // Arrange
        const int expectedDays = 7;

        // Act
        var result = await _weatherService.GetForecastAsync(expectedDays);

        // Assert
        Assert.Equal(expectedDays, result.Count());
    }

    [Fact]
    public async Task GetForecastAsync_DefaultDays_ReturnsFiveDays()
    {
        // Act
        var result = await _weatherService.GetForecastAsync();

        // Assert
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task GetForecastAsync_ReturnsValidWeatherData()
    {
        // Act
        var result = await _weatherService.GetForecastAsync(3);

        // Assert
        Assert.All(result, forecast =>
        {
            Assert.True(forecast.Date >= DateOnly.FromDateTime(DateTime.Now));
            Assert.InRange(forecast.TemperatureC, -20, 55);
            Assert.NotNull(forecast.Summary);
            Assert.NotEmpty(forecast.Summary);
        });
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_ReturnsValidWeatherData()
    {
        // Act
        var result = await _weatherService.GetCurrentWeatherAsync();

        // Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), result.Date);
        Assert.InRange(result.TemperatureC, -20, 55);
        Assert.NotNull(result.Summary);
        Assert.NotEmpty(result.Summary);
    }

    [Fact]
    public async Task GetForecastAsync_MultipleCalls_ReturnsDifferentData()
    {
        // Act
        var result1 = await _weatherService.GetForecastAsync(1);
        var result2 = await _weatherService.GetForecastAsync(1);

        // Assert
        var forecast1 = result1.First();
        var forecast2 = result2.First();
        
        // Due to randomness, at least one property should be different
        Assert.True(forecast1.TemperatureC != forecast2.TemperatureC || 
                   forecast1.Summary != forecast2.Summary);
    }
}