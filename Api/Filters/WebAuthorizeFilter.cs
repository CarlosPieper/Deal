using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Entities;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Filters
{
    public class WebAuthorizeFilter : IActionFilter
    {
        private ApplicationSettings _appSettings;
        public WebAuthorizeFilter(IOptions<ApplicationSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var req = context.HttpContext.Request;
            var res = context.HttpContext.Response;
            string authorization = req.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authorization))
            {
                context.Result = new UnauthorizedObjectResult("No token provided.");
                return;
            }

            var parts = authorization.Split(" ");

            if (parts.Length != 2)
            {
                context.Result = new UnauthorizedObjectResult("Invalid token");
                return;
            }

            var scheme = parts[0];
            var token = parts[1];

            if (!scheme.Contains("Bearer"))
                throw new Exception("Token malformatted.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(this._appSettings.JWTSecret);

            var handler = new JwtSecurityTokenHandler();

            var parametersValidation = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };

            SecurityToken validationToken;

            try
            {
                handler.ValidateToken(token, parametersValidation, out validationToken);
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedObjectResult("Invalid token. ");
                return;
            }

            if (validationToken == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid token.");
                return;
            }

            return;
        }
    }
}