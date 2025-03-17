using Castle.Core.Logging;
using FlagExplorerAppKC.API.Controllers;
using FlagExplorerAppKC.API.Models;
using FlagExplorerAppKC.API.Services.Interfaces;
using FlagExplorerAppKC.API.Services.Logic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using Moq.Protected;
using System.Net;
using System.Security.Claims;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FlagExplorerAppKC.Tests
{
    public class CountryControllerTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly HttpClient _httpClient;
        private readonly CountryController _countryController;
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;
        private readonly ITestOutputHelper _testOutput;

        public CountryControllerTests(ITestOutputHelper testOutput)
        {

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

            _logger = testOutput.BuildLoggerFor<CountryController>();
            _countryService = new CountryService(_httpClient);

            string user = "test", email = "test@test.com";
            _httpContextAccessorMock = this.SetupHttpContextAccessor(user, email);

            _countryController = new CountryController(_countryService, _httpContextAccessorMock.Object, _logger);
            _testOutput = testOutput;
        }

        [Fact]
        public async Task TestGet()
        {
            // Act
            var requestResult = await _countryController.Get();
            Assert.NotNull(requestResult);

            var results = ((Microsoft.AspNetCore.Mvc.ObjectResult)requestResult).Value as IEnumerable<Country>;
            Assert.NotNull(results);
            Assert.NotEmpty(results);
            results.AsParallel().ForAll(country =>
            {
                _testOutput.WriteLine($"Country: {country.Name}, Flag: {country.Flag}");
            });
        }

        [Fact]
        public async Task TestGetFromName()
        {
            string testCountry = "South Africa";

            // Act
            var requestResult = await _countryController.Get(testCountry);
            Assert.NotNull(requestResult);

            var result = ((Microsoft.AspNetCore.Mvc.ObjectResult)requestResult).Value as CountryDetails;
            Assert.NotNull(result);
            Assert.True(result.Population > 0);
            Assert.NotNull(result.Capital);
            _testOutput.WriteLine($"Country: {testCountry}, Capital: {result.Capital}, Population: {result.Population}");
        }

        private Mock<IHttpContextAccessor> SetupHttpContextAccessor(string userName, string userEmail)
        {
            var httpContextMock = new Mock<HttpContext>();
            var httpResponseMock = new Mock<HttpResponse>();
            var httpRequestMock = new Mock<HttpRequest>();
            var httpFeaturesMock = new Mock<IFeatureCollection>();
            var authenticationManagerMock = new Mock<IAuthenticationService>();

            // Setup User Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, userEmail)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // Or JwtBearerDefaults.AuthenticationScheme, etc.
            var principal = new ClaimsPrincipal(identity);
            var authenticationResult = AuthenticateResult.Success(new AuthenticationTicket(principal, CookieAuthenticationDefaults.AuthenticationScheme));

            // Setup Authentication
            authenticationManagerMock.Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>())).ReturnsAsync(authenticationResult);

            //Setup HttpContext
            httpContextMock.Setup(x => x.User).Returns(principal);
            httpContextMock.Setup(x => x.Response).Returns(httpResponseMock.Object);
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);
            httpContextMock.Setup(x => x.Features).Returns(httpFeaturesMock.Object);
            httpContextMock.Setup(x => x.RequestServices.GetService(typeof(IAuthenticationService))).Returns(authenticationManagerMock.Object);

            // Setup HttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            return httpContextAccessorMock;
        }
    }
}
