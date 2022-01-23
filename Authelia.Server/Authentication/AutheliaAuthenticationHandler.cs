using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Encodings.Web;
using Authelia.Database.Model;
using Authelia.Server.Security;

namespace Authelia.Server.Authentication
{
    public class AutheliaAuthenticationHandler : AuthenticationHandler<AutheliaAuthenticationOptions>
    {
        private AutheliaDbContext dbContext;
        private IPasswordSecurer passwordSecurer;

        public AutheliaAuthenticationHandler(IOptionsMonitor<AutheliaAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, AutheliaDbContext dbContext, IPasswordSecurer passwordSecurer) : base(options, logger, encoder, clock)
        {
            this.dbContext = dbContext;
            this.passwordSecurer = passwordSecurer;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
            }

            var header = Request.Headers[HeaderNames.Authorization].ToString();
            var password = passwordSecurer.Secure(header);
            var userToken = dbContext.UserTokens
                .Include(x => x.User)
                .FirstOrDefault(x => x.UserTokenId == password);

            if (userToken == null)
                return Task.FromResult(AuthenticateResult.Fail("token not found"));

            if (userToken.TokenExpiration != null && userToken.TokenExpiration <= DateTime.UtcNow)
                return Task.FromResult(AuthenticateResult.Fail("token is expired"));

            var scheme = AutheliaSchemes.AutheliaScheme;
            var ticket = new AuthenticationTicket(userToken.User.CreatePrincipal(scheme), scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
