# Q Developer Demo - .NET 9 ASP.NET Core Project

A modern .NET 9 ASP.NET Core Web API project demonstrating best practices for project structure, dependency injection, testing, and API development.

## Project Structure

```
QDeveloperDemo/
├── src/
│   └── QDeveloperDemo.Web/           # Main web application
│       ├── Controllers/              # API controllers
│       ├── Models/                   # Data models
│       ├── Services/                 # Business logic services
│       ├── Properties/               # Launch settings
│       ├── Program.cs                # Application entry point
│       ├── appsettings.json          # Configuration
│       └── QDeveloperDemo.Web.csproj # Project file
├── tests/
│   └── QDeveloperDemo.Tests/         # Unit and integration tests
│       ├── Controllers/              # Controller tests
│       ├── Services/                 # Service tests
│       ├── Integration/              # Integration tests
│       └── QDeveloperDemo.Tests.csproj # Test project file
├── QDeveloperDemo.sln                # Solution file
├── global.json                       # .NET SDK version
├── Directory.Build.props             # Common MSBuild properties
└── .gitignore                        # Git ignore file
```

## Features

- **ASP.NET Core Web API** with .NET 9
- **Swagger/OpenAPI** documentation
- **Dependency Injection** with service registration
- **Configuration Management** with appsettings.json
- **Comprehensive Testing** with xUnit, Moq, and integration tests
- **Clean Architecture** with separation of concerns

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 or Visual Studio Code (optional)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd q-developer-demo
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Build the solution

```bash
dotnet build
```

### 4. Run the application

```bash
dotnet run --project src/QDeveloperDemo.Web
```

The application will start and be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7000
- Swagger UI: https://localhost:7000/swagger

### 5. Run tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## API Endpoints

### Weather Forecast API

- `GET /WeatherForecast` - Get 5-day weather forecast
- `GET /WeatherForecast/current` - Get current weather
- `GET /WeatherForecast/{days}` - Get weather forecast for specified days (1-30)

### Example Requests

```bash
# Get default 5-day forecast
curl https://localhost:7000/WeatherForecast

# Get current weather
curl https://localhost:7000/WeatherForecast/current

# Get 10-day forecast
curl https://localhost:7000/WeatherForecast/10
```

## Configuration

The application uses standard ASP.NET Core configuration:

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development-specific settings
- Environment variables
- Command line arguments

### Weather Settings

```json
{
  "WeatherSettings": {
    "DefaultLocation": "Seattle",
    "TemperatureUnit": "Celsius"
  }
}
```

## Development

### Adding New Services

1. Create interface in `Services/` folder
2. Implement the interface
3. Register in `Program.cs`:

```csharp
builder.Services.AddScoped<IYourService, YourService>();
```

### Adding New Controllers

1. Create controller in `Controllers/` folder
2. Inherit from `ControllerBase`
3. Add `[ApiController]` and `[Route]` attributes

### Testing

The project includes three types of tests:

1. **Unit Tests** - Test individual components in isolation
2. **Integration Tests** - Test the full application stack
3. **Service Tests** - Test business logic services

## Technologies Used

- **.NET 9** - Latest .NET framework
- **ASP.NET Core** - Web framework
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Testing framework
- **Moq** - Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.