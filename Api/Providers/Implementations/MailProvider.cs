using Api.Entities;
using Api.Providers.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Api.Providers.Implementations
{
    public class MailProvider : IMailProvider
    {
        private ApplicationSettings _settings;
        public MailProvider(IOptions<ApplicationSettings> settings)
        {
            this._settings = settings.Value;
        }

        public void SendMail(MailMessage message)
        {
            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress from = new MailboxAddress(message.From.Name, message.From.Email);
            mimeMessage.From.Add(from);

            MailboxAddress to = new MailboxAddress(message.To.Name, message.To.Email);
            mimeMessage.To.Add(to);

            mimeMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.Body
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(this._settings.DefaultEmail, this._settings.DefaultPassword);
            client.Send(mimeMessage);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}