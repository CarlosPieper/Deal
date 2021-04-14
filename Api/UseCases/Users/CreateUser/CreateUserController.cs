using System;
using System.Text.RegularExpressions;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.UseCases.Users.CreateUser
{
    [ApiController]
    [Route("CreateUser")]
    public class CreateUserController : Controller
    {
        private IUserRepository _repository;
        private ICryptographyService _cryptographyService;
        public CreateUserController(IUserRepository repository, ICryptographyService cryptographyService)
        {
            this._repository = repository;
            this._cryptographyService = cryptographyService;
        }

        [HttpPost]
        public IActionResult Execute(CreateUserDTO data)
        {
            try
            {
                var isValidEmail = Regex.IsMatch(data.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                if (!isValidEmail)
                {
                    return BadRequest(new { message = "Invalid Email." });
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return BadRequest(new { message = "Invalid Email." });
            }

            if (data.Password != data.PasswordConfirmation)
            {
                return BadRequest(new { message = "Password confirmation does not match with password." });
            }

            User user = new User(data);
            user.Password = this._cryptographyService.EncryptPassword(user.Password);

            try
            {
                this._repository.Create(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok();
        }
    }
}
