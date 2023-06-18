using System.Diagnostics;

namespace OnlineShopASP
{
    public class AppStartedNotificatorBackgroundService : BackgroundService
    {     
        private readonly IEmailSender _emailSender;

        public AppStartedNotificatorBackgroundService(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
                  
            _emailSender.SendEmailAsync("to_address@example.com", "Приложение запущено", "Приложение запущено");
        }

    }
}
