using FlagExplorerAppKC.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlagExplorerAppKC.API.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public ApiBaseController(IHttpContextAccessor httpContextAccessor, ILogger logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ObjectResult Error()
        {
            return StatusCode(500, "The server encountered an internal error and was unable to complete your request.");
        }

        public string RequestedUrl
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Request?.Path.Value ?? "Unknown URL";
            }
        }

        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimType.UserName.ToString())?.Value ?? "Unknown";
            }
        }

        public string LogErrorMessage
        {
            get
            {
                string url = _httpContextAccessor.HttpContext?.Request?.Path.Value ?? "Unknown URL";
                return $"Error 500. The requested URL ({url}) encountered an issue.";
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void LogRequest(string method, string? name = null)
        {
            string message = !string.IsNullOrEmpty(name)
                ? $"{method} request for Name {name} started at {RequestedUrl} by {UserName}"
                : $"{method} request started at {RequestedUrl} by {UserName}";

            _logger.LogInformation(message);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void LogSuccess(string method, string? name = null)
        {
            string message = !string.IsNullOrEmpty(name)
                ? $"{method} request for Name {name} successfully processed at {RequestedUrl}"
                : $"{method} request successfully processed at {RequestedUrl}";

            _logger.LogInformation(message);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void LogError(Exception ex)
        {
            _logger.LogError(LogErrorMessage, ex);
        }
    }
}
