namespace ApplicationServices.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(MailData mailData);
    }
}
