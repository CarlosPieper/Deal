using System;
using System.Text.RegularExpressions;
using Api.Entities;
using Api.Models;
using Api.Providers.Interfaces;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.UseCases.Users.RecoverUserPassword
{
    [ApiController]
    [Route("RecoverUserPassword")]
    public class RecoverUserPasswordController : Controller
    {
        private IUserRepository _repository;
        private ICryptographyService _cryptographyService;
        private IMailProvider _mailProvider;
        public RecoverUserPasswordController(
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
        public IActionResult Execute(string email)
        {
            try
            {
                var user = this._repository.FindByEmail(email);

                if (user == null)
                    return BadRequest(new { message = "Email not registered." });

                string password = Guid.NewGuid().ToString().Substring(0, 8);
                user.Password = this._cryptographyService.EncryptPassword(password);

                this._repository.Update(user);
                this._mailProvider.SendMail(
                    new MailMessage
                    {
                        Subject = "Your new password",
                        Body = $"Hello, your new password is {password}, use it to log in and then change it",
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

                return Ok(new { message = "We sent you an Email containing your new password" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}