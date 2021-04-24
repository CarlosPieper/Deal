namespace Api.Entities
{
    public class MailMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public MailAdress From { get; set; }
        public MailAdress To { get; set; }
    }
}