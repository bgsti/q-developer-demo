using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using QDeveloperDemo.Web.Controllers;
using QDeveloperDemo.Web.Models;
using QDeveloperDemo.Web.Services;

namespace QDeveloperDemo.Tests.Controllers;

public class WeatherForecastControllerTests
{
    private readonly Mock<ILogger<WeatherForecastController>> _mockLogger;
    private readonly Mock<IWeatherService> _mockWeatherService;
    private readonly WeatherForecastController _controller;

    public WeatherForecastControllerTests()
    {
        _mockLogger = new Mock<ILogger<WeatherForecastController>>();
        _mockWeatherService = new Mock<IWeatherService>();
        _controller = new WeatherForecastController(_mockLogger.Object, _mockWeatherService.Object);
    }

    [Fact]
    public async Task Get_ReturnsWeatherForecast()
    {
        // Arrange
        var expectedForecast = new List<WeatherForecast>
        {
            new() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 20, Summary = "Mild" },
            new() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), TemperatureC = 25, Summary = "Warm" }
        };

        _mockWeatherService.Setup(s => s.GetForecastAsync(It.IsAny<int>()))
            .ReturnsAsync(expectedForecast);

        // Act
        var result = await _controller.Get();

        // Assert
        Assert.Equal(expectedForecast, result);
        _mockWeatherService.Verify(s => s.GetForecastAsync(5), Times.Once);
    }

    [Fact]
    public async Task GetCurrent_ReturnsCurrentWeather()
    {
        // Arrange
        var expectedWeather = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 22,
            Summary = "Pleasant"
        };

        _mockWeatherService.Setup(s => s.GetCurrentWeatherAsync())
            .ReturnsAsync(expectedWeather);

        // Act
        var result = await _controller.GetCurrent();

        // Assert
        Assert.Equal(expectedWeather, result);
        _mockWeatherService.Verify(s => s.GetCurrentWeatherAsync(), Times.Once);
    }

    [Fact]
    public async Task GetForecast_WithValidDays_ReturnsWeatherForecast()
    {
        // Arrange
        const int days = 10;
        var expectedForecast = new List<WeatherForecast>
        {
            new() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 15, Summary = "Cool" }
        };

        _mockWeatherService.Setup(s => s.GetForecastAsync(days))
            .ReturnsAsync(expectedForecast);

        // Act
        var result = await _controller.GetForecast(days);

        // Assert
        Assert.Equal(expectedForecast, result);
        _mockWeatherService.Verify(s => s.GetForecastAsync(days), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(31)]
    [InlineData(100)]
    public async Task GetForecast_WithInvalidDays_ThrowsArgumentOutOfRangeException(int invalidDays)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _controller.GetForecast(invalidDays));
        _mockWeatherService.Verify(s => s.GetForecastAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetForecast_WithBoundaryValues_ReturnsWeatherForecast()
    {
        // Arrange
        var expectedForecast = new List<WeatherForecast>();
        _mockWeatherService.Setup(s => s.GetForecastAsync(It.IsAny<int>()))
            .ReturnsAsync(expectedForecast);

        // Act & Assert - Test boundary values
        await _controller.GetForecast(1); // Minimum valid value
        await _controller.GetForecast(30); // Maximum valid value

        _mockWeatherService.Verify(s => s.GetForecastAsync(1), Times.Once);
        _mockWeatherService.Verify(s => s.GetForecastAsync(30), Times.Once);
    }
}