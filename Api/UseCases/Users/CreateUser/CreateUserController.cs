using System;
using System.Text.RegularExpressions;
using Api.Entities;
using Api.Models;
using Api.Providers.Interfaces;
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
        private IMailProvider _mailProvider;
        public CreateUserController(
            IUserRepository repository,
            ICryptographyService cryptographyService,
            IMailProvider mailProvider
            )
        {
            this._repository = repository;
            this._cryptographyService = cryptographyService;
            this._mailProvider = mailProvider;
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
                return BadRequest(new { message = "Password confirmation does not match the password." });
            }

            User user = new User(data);
            user.Password = this._cryptographyService.EncryptPassword(user.Password);

            try
            {
                var userAlreadyExisting = this._repository.FindByEmail(user.Email);

                if (userAlreadyExisting != null)
                    return BadRequest(new { message = "Email already registered." });

                this._repository.Create(user);
                this._mailProvider.SendMail(
                    new MailMessage
                    {
                        Subject = "Welcome to the platform!",
                        Body = "Congratulations! Your account is now ready.",
                        From = new MailAdress
                        {
                            Name = "Deal Administration",
                            Email = "contact.atdeal@gmail.com",
                        },
                        To = new MailAdress
                        {
                            Name = user.Name,
                            Email = user.Email,
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok();
        }
    }
}
