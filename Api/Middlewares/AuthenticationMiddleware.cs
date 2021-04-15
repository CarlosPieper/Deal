using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationSettings _appSettings;
        public AuthenticationMiddleware(RequestDelegate next, IOptions<ApplicationSettings> appSettings)
        {
            this._next = next;
            this._appSettings = appSettings.Value;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var authorizations = httpContext.Request.Headers["Authorization"];
            var path = httpContext.Request.Path;

            //if (path == "/AuthenticateUser" || path == "/CreateUser")
            //    return _next(httpContext);

            if (authorizations.Count == 0)
                throw new Exception("No token provided.");

            var authorization = authorizations[0];

            var parts = authorization.Split(" ");

            if (parts.Length != 2)
                throw new Exception("Token error.");

            var scheme = parts[0];
            var token = parts[1];

            if (!scheme.Contains("Bearer"))
                throw new Exception("Token malformatted.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(this._appSettings.JWTSecret);
            
            SecurityToken validToken;
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                }, out validToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid token " + ex.Message);
            }

            if (validToken == null)
                throw new Exception("Invalid token ");

            return _next(httpContext);
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}