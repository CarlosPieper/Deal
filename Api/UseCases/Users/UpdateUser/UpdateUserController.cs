using System;
using System.Text.RegularExpressions;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.UseCases.Users.UpdateUser
{
    [ApiController]
    [Route("UpdateUser")]
    public class UpdateUserController : Controller
    {
        private IUserRepository _repository;
        private ICryptographyService _cryptographyService;
        public UpdateUserController(IUserRepository repository, ICryptographyService cryptographyService)
        {
            this._repository = repository;
            this._cryptographyService = cryptographyService;
        }

        [HttpPost]
        public IActionResult Execute(UpdateUserDTO data)
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

            if (data.NewPassword != data.NewPasswordConfirmation)
            {
                return BadRequest(new { message = "Password confirmation does not match the password." });
            }

            User user = new User(data);
            user.Password = this._cryptographyService.EncryptPassword(user.Password);

            try
            {
                this._repository.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok();
        }
    }
}
