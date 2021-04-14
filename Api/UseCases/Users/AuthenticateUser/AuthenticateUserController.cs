using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.UseCases.Users.AuthenticateUser
{
    [ApiController]
    [Route("AuthenticateUser")]
    public class AuthenticateUserController : Controller
    {
        private IUserRepository _repository;
        private readonly ApplicationSettings _appSettings;
        private ICryptographyService _cryptographyService;
        public AuthenticateUserController(
            IUserRepository repository,
            IOptions<ApplicationSettings> appSettings,
            ICryptographyService cryptographyService
            )
        {
            this._repository = repository;
            this._appSettings = appSettings.Value;
            this._cryptographyService = cryptographyService;
        }

        [HttpPost]
        public IActionResult Execute(AuthenticateUserDTO data)
        {
            try
            {
                User user = this._repository.FindByEmail(data.Email);
                if (user == null)
                    return BadRequest(new { message = "Email not found." });

                string password = this._cryptographyService.EncryptPassword(data.Password);
                if (user.Password == password)
                {
                    var token = this.CreateToken(user);
                    user.Password = "";
                    return Ok(new { user, token });
                }
                else
                    return BadRequest(new { message = "Wrong password." });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string CreateToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", user.Id)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWTSecret)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}