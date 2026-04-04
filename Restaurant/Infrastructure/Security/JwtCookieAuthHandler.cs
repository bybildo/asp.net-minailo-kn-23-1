using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurant.Application.Interfaces;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Restaurant.Infrastructure.Security
{
    public class JwtCookieAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IJwtService _jwtService;

        public JwtCookieAuthHandler(
            IJwtService jwtService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
            _jwtService = jwtService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.TryGetValue("jwt_token", out var token))
                return Task.FromResult(AuthenticateResult.NoResult());

            var principal = _jwtService.ValidateToken(token);

            if (principal == null)
                return Task.FromResult(AuthenticateResult.Fail("Invalid or expired token"));

            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 401;
            await Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status = 401,
                message = "You are not authenticated. Please log in."
            }));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 403;
            await Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status = 403,
                message = "You do not have permission to access this resource."
            }));
        }
    }
}
