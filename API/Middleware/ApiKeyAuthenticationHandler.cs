using API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace API.Middleware
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string API_KEY_HEADER_NAME = "X-API-KEY";
        private readonly APIKeyService _apiKeyService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            APIKeyService apiKeyService)
            : base(options, logger, encoder)
        {
            _apiKeyService = apiKeyService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.Fail("API Key was not provided");
            }

            var providedApiKey = apiKeyHeaderValues.ToString();
            var apiKeyResult = await _apiKeyService.CollectAPIDetail(providedApiKey);

            if(apiKeyResult is null)
            {
                return AuthenticateResult.Fail("Invalid API Key provided.");
            }

            var claims = new[]
            {
                new Claim("val", "true"),
            };

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
            
        }
    }

}
