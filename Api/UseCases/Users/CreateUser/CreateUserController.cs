using System;
using System.Text.RegularExpressions;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.UseCases.Users.CreateUser
{
    [ApiController]
    [Route("CreateUser")]
    public class CreateUserController : Controller
    {
        public CreateUserController()
        {

        }

        [HttpPost]
        public IActionResult Execute(CreateUserDTO user)
        {
            this.ValidateUserCreation(user);

            User userBeingCreated = new User(user);

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
