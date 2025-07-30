using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using QDeveloperDemo.Web.Models;

namespace QDeveloperDemo.Tests.Integration;

public class WeatherForecastIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WeatherForecastIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_WeatherForecast_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Get_WeatherForecast_ReturnsValidJson()
    {
        // Act
        var response = await _client.GetAsync("/WeatherForecast");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        
        var forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts.Length); // Default is 5 days
        
        foreach (var forecast in forecasts)
        {
            Assert.True(forecast.Date >= DateOnly.FromDateTime(DateTime.Now));
            Assert.InRange(forecast.TemperatureC, -20, 55);
            Assert.NotNull(forecast.Summary);
        }
    }

    [Fact]
    public async Task Get_CurrentWeather_ReturnsSuccessAndValidData()
    {
        // Act
        var response = await _client.GetAsync("/WeatherForecast/current");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        
        var currentWeather = JsonSerializer.Deserialize<WeatherForecast>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(currentWeather);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), currentWeather.Date);
        Assert.InRange(currentWeather.TemperatureC, -20, 55);
        Assert.NotNull(currentWeather.Summary);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(30)]
    public async Task Get_WeatherForecastWithDays_ReturnsCorrectNumberOfDays(int days)
    {
        // Act
        var response = await _client.GetAsync($"/WeatherForecast/{days}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        
        var forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(forecasts);
        Assert.Equal(days, forecasts.Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(31)]
    public async Task Get_WeatherForecastWithInvalidDays_ReturnsBadRequest(int invalidDays)
    {
        // Act
        var response = await _client.GetAsync($"/WeatherForecast/{invalidDays}");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Get_SwaggerEndpoint_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/swagger/index.html");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }
}