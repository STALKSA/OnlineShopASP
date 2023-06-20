using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace OnlineShopASP
{
    public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly SmtpClient _smtpClient = new();


        public async ValueTask DisposeAsync()
        {
            await _smtpClient.DisconnectAsync(true);
            _smtpClient.Dispose();
        }

        public async Task SendEmail(string recepientEmail, string subject, string body)
        {
            ArgumentNullException.ThrowIfNull(recepientEmail);
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(body);

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
            await EnsureConnectAndAuthenticateAsync();
            await _smtpClient.SendAsync(email);

        }


        private async Task EnsureConnectAndAuthenticateAsync()
        {
            if(!_smtpClient.IsConnected)
            {
               await _smtpClient.ConnectAsync("smtp.ethereal.email", 25);
            }
            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync("[USERNAME]", "[PASSWORD]");
            }
        
        }

        private Task DisconnectAsync()
        {
            return _smtpClient.DisconnectAsync(true);
        }

        
    }
}
