using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace OnlineShopASP
{
    public class MailKitSmtpEmailSender : IEmailSender
    {

        public async Task SendEmailAsync(string recepientEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(recepientEmail))
            {
                throw new ArgumentException("Поле 'Получатель' не может быть пустым", nameof(recepientEmail));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("Поле 'Тема письма' не может быть пустым", nameof(subject));
            }

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException("Письмо не может быть пустым", nameof(body));
            }

            var email = new MimeMessage()
            {
                Subject = subject,
                Body = new TextPart(TextFormat.Plain)
                {
                    Text = body
                },
                From = { MailboxAddress.Parse("from_address@example.com") },
                To = { (MailboxAddress.Parse(recepientEmail)) }
            };



            // отправка email
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.ethereal.email", 25);
                await smtp.AuthenticateAsync("[USERNAME]", "[PASSWORD]");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }


        }
    }
}
