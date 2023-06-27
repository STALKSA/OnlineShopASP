using OnlineShopASP.Interfaces;
using System.Diagnostics;

namespace OnlineShopASP.BackgroundServices
{
    public class AppStartedNotificatorBackgroundService : BackgroundService
    {
        // private readonly IEmailSender _emailSender;
        private readonly ILogger<AppStartedNotificatorBackgroundService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public AppStartedNotificatorBackgroundService(IEmailSender emailSender, ILogger<AppStartedNotificatorBackgroundService> logger,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            //  _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            hostApplicationLifetime.ApplicationStarted.Register(() => logger.LogInformation("Приложение запущено"));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            //  _logger.LogInformation("Приложение запущено...");

        }

    }
}
