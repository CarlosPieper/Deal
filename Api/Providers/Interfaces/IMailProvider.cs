using Api.Entities;

namespace Api.Providers.Interfaces
{
    public interface IMailProvider
    {
        void SendMail(MailMessage message);
    }
}