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
        public IActionResult Execute(CreateUserDTO user)
        {
            this.ValidateUserCreation(user);

            var userBeingCreated = new User(user);
            try
            {
                this._repository.Create(userBeingCreated);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok();
        }

        public void ValidateUserCreation(CreateUserDTO user)
        {
            try
            {
                var isValidEmail = Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                if (!isValidEmail)
                {
                    throw new Exception("Invalid Email.");
                }
            }
            catch (RegexMatchTimeoutException)
            {
                throw new Exception("Invalid Email.");
            }

            if (user.Password != user.PasswordConfirmation)
            {
                throw new Exception("Password confirmation does not match with password.");
            }
        }
    }
}
