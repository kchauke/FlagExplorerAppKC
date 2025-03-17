using FlagExplorerAppKC.API.Services.Logic;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FlagExplorerAppKC.Tests
{
    public class CountryServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly CountryService _countryService;
        private readonly ITestOutputHelper _testOutput;


        public CountryServiceTests(ITestOutputHelper testOutput)
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(); _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            // Load configuration from appsettings.Test.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var baseAddress = configuration["ApiSettings:RestCountriesUrl"];

            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException(nameof(baseAddress), "Base address cannot be null or empty.");
            }

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };

            _countryService = new CountryService(_httpClient);
            _testOutput = testOutput;
        }

        [Fact]
        public async Task TestGetAllCountriesAsync()
        {
            // Act
            var result = await _countryService.GetAllCountriesAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            result.AsParallel().ForAll(country =>
            {
                _testOutput.WriteLine($"Country: {country.Name}, Flag: {country.Flag}");
            });
        }

        [Fact]
        public async Task TestGetCountryDetailsAsync()
        {
            string testCountry = "South Africa";

            // Act
            var result = await _countryService.GetCountryDetailsAsync(testCountry);

            Assert.NotNull(result);
            Assert.True(result.Population > 0);
            Assert.NotNull(result.Capital);
            _testOutput.WriteLine($"Country: {testCountry}, Capital: {result.Capital}, Population: {result.Population}");
        }
    }
}
