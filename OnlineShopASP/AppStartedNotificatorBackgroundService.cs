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
            while (!stoppingToken.IsCancellationRequested)
          {
            // Отправка письма
            await _emailSender.SendEmailAsync("to_address@example.com", "Сервер работает исправно", "Сервер работает исправно");

            // Задержка на 1 час
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
          }
          
        }

    }
}
