using SuperDigital.DigitalAccount.Api.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SuperDigital.DigitalAccount.Api
{
    public partial class Startup
    {
        private static readonly string secretKey = "8d200898-0e4b-4897-855a-d5d999f6fc1b";

        private void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();

            var signingKey = new SymmetricSecurityKey(ASCIIEncoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = "SuperDigitalAudience",
                ValidateAudience = true,
                ValidAudience = "SuperDigitalIssuer",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            app.UseSimpleTokenProvider(new TokenProviderOptions
            {
                Path = "/token",
                Audience = "SuperDigitalAudience",
                Issuer = "SuperDigitalIssuer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity,
                TokenKey = Configuration.GetValue<string>("SuperDigital.DigitalAccount.Api:Key")
            }, tokenValidationParameters);
        }

        private Task<User> GetIdentity(IUserService userService, string username, string password)
        {
            User user = userService.Authenticate(username, password);

            if (user == null)
                return Task.FromResult<User>(null);

            return Task.FromResult(user);
        }
    }
}
