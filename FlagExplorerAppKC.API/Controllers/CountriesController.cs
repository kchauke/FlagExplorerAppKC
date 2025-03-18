using FlagExplorerAppKC.API.Models;
using FlagExplorerAppKC.API.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlagExplorerAppKC.API.Controllers;

[ApiController]
[Route("countryapi/[controller]")]
[Produces("application/json")]
public class CountriesController : ApiBaseController
{
    private readonly ILogger<CountriesController> _logger;

    private readonly ICountryService _countryService;

    public CountriesController(ICountryService countryService, IHttpContextAccessor httpContextAccessor, ILogger<CountriesController> logger)
    : base(httpContextAccessor, logger)
    {
        _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
        _logger = logger;
    }

    [HttpGet(Name = "countries")]
    [ProducesResponseType(typeof(Country[]), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Retrieve all countries.", Description = "A list of countries.")]
    public async Task<IActionResult> Get()
    {
        LogRequest("GET");
        try
        {
            var result = await _countryService.GetAllCountriesAsync();

            LogSuccess("GET");
            return Ok(result);
        }
        catch (Exception ex)
        {
            LogError(ex);
            return Error();
        }
    }

    [HttpGet("{name}", Name = "countries{name}")]
    [ProducesResponseType(typeof(CountryDetails), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Retrieve details about a specific country.", Description = "Details about the country.")]
    public async Task<IActionResult> Get(string name)
    {
        LogRequest("GET");
        try
        {
            var result = await _countryService.GetCountryDetailsAsync(name);

            LogSuccess("GET");
            return Ok(result);
        }
        catch (Exception ex)
        {
            LogError(ex);
            return Error();
        }
    }
}
