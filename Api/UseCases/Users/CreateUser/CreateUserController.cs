using System;
using System.Text.RegularExpressions;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.UseCases.Users.CreateUser
{
    [ApiController]
    [Route("CreateUser")]
    public class CreateUserController : Controller
    {
        private IUserRepository _repository;
        public CreateUserController(IUserRepository repository)
        {
            this._repository = repository;
        }

        [HttpPost]
        public IActionResult Execute(CreateUserDTO data)
        {
            this.ValidateUserCreation(data);

            User user = new User(data);

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

        public void ValidateUserCreation(CreateUserDTO data)
        {
            try
            {
                var isValidEmail = Regex.IsMatch(data.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                if (!isValidEmail)
                {
                    throw new Exception("Invalid Email.");
                }
            }
            catch (RegexMatchTimeoutException)
            {
                throw new Exception("Invalid Email.");
            }

            if (data.Password != data.PasswordConfirmation)
            {
                throw new Exception("Password confirmation does not match with password.");
            }
        }
    }
}
