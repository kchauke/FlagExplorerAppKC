namespace FlagExplorerAppKC.Tests
{
    using Moq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using System.Security.Claims;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class MockTests
    {
        private readonly ITestOutputHelper _testOutput;

        public MockTests(ITestOutputHelper testOutputHelper)
        {
            _testOutput = testOutputHelper;
        }

        public Mock<IHttpContextAccessor> SetupHttpContextAccessor(string userName, string userEmail)
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

        [Fact]
        public async Task TestHttpContextAccessorSetup()
        {
            var userName = "test ing";
            var userEmail = "test@test.com";

            var httpContextAccessorMock = SetupHttpContextAccessor(userName, userEmail);

            Assert.NotNull(httpContextAccessorMock.Object.HttpContext);
            Assert.NotNull(httpContextAccessorMock.Object.HttpContext.User);
            Assert.NotNull(httpContextAccessorMock.Object.HttpContext.User.Identity);
            Assert.NotNull(httpContextAccessorMock.Object.HttpContext.User.Identity.Name);
            _testOutput.WriteLine(httpContextAccessorMock.Object.HttpContext.User.Identity.Name);

            Assert.Equal(userName, httpContextAccessorMock.Object.HttpContext.User.Identity.Name);
            Assert.Equal(userEmail, httpContextAccessorMock.Object.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value);
            Assert.True(httpContextAccessorMock.Object.HttpContext.User.Identity.IsAuthenticated);

            var authenticationService = httpContextAccessorMock.Object.HttpContext.RequestServices.GetService(typeof(IAuthenticationService)) as IAuthenticationService;
            Assert.NotNull(authenticationService);
            var authenticationResult = await authenticationService.AuthenticateAsync(httpContextAccessorMock.Object.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme);
            Assert.True(authenticationResult.Succeeded);
        }
    }
}
