using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using SuperDigital.DigitalAccount.Api.Models;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;

namespace SuperDigital.DigitalAccount.Api.Provider
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IUserService _userService;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            TokenValidationParameters tokenValidationParameters,
            ILoggerFactory loggerFactory, IUserService userService)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TokenProviderMiddleware>();
            _tokenValidationParameters = tokenValidationParameters;
            _userService = userService;

            _options = options.Value;
            ThrowIfInvalidOptions(_options);

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke(HttpContext context)
        {
            bool isCreate = context.Request.Path.Equals(_options.Path, StringComparison.Ordinal);
           
            if (!isCreate)
            {
                return _next(context);
            }

            if (!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            return GenerateToken(context);
        }

        private Task IssueRefreshedToken(HttpContext context)
        {
            try
            {
                string authenticationText = context.Request.Headers["Authorization"].ToString();
                int firstSpace = authenticationText.IndexOf(" ");
                string token = authenticationText.Substring(firstSpace + 1);

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                SecurityToken originalToken;
               
                var claimsi = jwtSecurityTokenHandler.ValidateToken(token, _tokenValidationParameters, out originalToken);
                var now = DateTime.UtcNow;
                
                var jwt = new JwtSecurityToken(
                 issuer: _options.Issuer,
                 audience: _options.Audience,
                 claims: ((JwtSecurityToken)originalToken).Claims,
                 notBefore: now,
                 expires: now.Add(_options.Expiration),
                 signingCredentials: _options.SigningCredentials);
                return WriteTokenResponse(context, jwt);
            }
            catch
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Invalid token.");
            }
        }

        private async Task GenerateToken(HttpContext context)
        {
            var grantType = context.Request.Form["grant_type"];
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];
            string key = context.Request.Headers["Authorization"];

            if(!string.IsNullOrEmpty(key))
            {
                int firstSpace = key.IndexOf(" ");
                key = key.Substring(firstSpace + 1);
            }
            
            if (grantType != GrantType.UserCredentials)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid grantType.");
                return;
            }   

            if (key != _options.TokenKey)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Authorization basic invalid value.");
                return;
            }
                
            var identity = await _options.IdentityResolver(_userService,username, password);
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;

            List<Claim> claims = new ClaimModel(identity).Get();

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds,
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }

        private async Task WriteTokenResponse(HttpContext context, JwtSecurityToken jwt)
        {
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
            }
        }

        public static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }

    public static class GrantType
    {
        public static string UserCredentials { get; set; } = "user_credentials";
    }
}
