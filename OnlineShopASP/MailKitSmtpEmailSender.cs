using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using OnlineShopASP.Interfaces;

namespace OnlineShopASP
{
    public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly SmtpClient _smtpClient = new();
        private readonly SmtpConfig _smtpConfig;

        public MailKitSmtpEmailSender(IOptionsSnapshot<SmtpConfig> options)
        {
            
            ArgumentNullException.ThrowIfNull(nameof(options));
            _smtpConfig = options.Value;
        }

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
                From = { MailboxAddress.Parse(_smtpConfig.FromEmail) },
                To = { (MailboxAddress.Parse(recepientEmail)) }
            };

            await EnsureConnectAndAuthenticateAsync();
            await _smtpClient.SendAsync(email);

        }


        private async Task EnsureConnectAndAuthenticateAsync()
        {
            

            if (!_smtpClient.IsConnected)
            {
               await _smtpClient.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false);
            }
            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
            }
        
        }

        //private Task DisconnectAsync()
        //{
        //    return _smtpClient.DisconnectAsync(true);
        //}

        
    }
}
