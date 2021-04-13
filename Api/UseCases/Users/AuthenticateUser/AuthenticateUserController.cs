using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly ApplicationSettings _appSettings;
        public AuthenticateUserController(
            IUserRepository repository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<ApplicationSettings> appSettings
            )
        {
            this._repository = repository;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Execute(AuthenticateUserDTO data)
        {
            try
            {
                User user = await this._repository.FindByEmail(data.Email);
                if (user == null)
                    return BadRequest("Email not found.");

                if (await _userManager.CheckPasswordAsync(user, data.Password))
                {
                    var token = this.CreateToken(user);
                    user.Password = "";
                    return Ok(new { user, token });
                    //methods that have to be authenticated just need the flas [Authorize] before executing
                }
                else
                    return BadRequest("Wrong password.");
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