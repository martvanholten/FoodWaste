using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
namespace ApplicationServices.Logic
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings mailSettings;
        public EmailService(IOptions<MailSettings> mailSettingsOptions)
        {
            mailSettings = mailSettingsOptions.Value;
        }

        public async Task SendEmailAsync(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(mailSettings.SenderName, mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(mailSettings.Server, mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(mailSettings.UserName, mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }
            }
            catch
            {
                throw new ErrorModel($"Er is iets fout gegaan", 400);
            }
        }
    }
}
