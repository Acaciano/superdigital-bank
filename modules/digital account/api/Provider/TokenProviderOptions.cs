using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Api.Provider
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(60);
        public SigningCredentials SigningCredentials { get; set; }
        public Func<IUserService, string, string, Task<User>> IdentityResolver { get; set; }
        public Func<Task<string>> NonceGenerator { get; set; } = new Func<Task<string>>(() => Task.FromResult(Guid.NewGuid().ToString()));
        public string TokenKey { get; internal set; }
    }
}