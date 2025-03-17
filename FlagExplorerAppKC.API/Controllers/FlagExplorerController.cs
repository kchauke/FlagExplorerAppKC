using FlagExplorerAppKC.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlagExplorerAppKC.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FlagExplorerController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<FlagExplorerController> _logger;

    public FlagExplorerController(ILogger<FlagExplorerController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("{searchText}", Name = "SearchWeatherForecast{searchText}")]
    public IEnumerable<WeatherForecast> Get(string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
        {
            return new List<WeatherForecast>().ToArray();
        }

        return Summaries.Where(x=> x.Contains(searchText, StringComparison.OrdinalIgnoreCase)).Select(y => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(Random.Shared.Next(1,5))),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = y
        }).ToArray();
    }
}
