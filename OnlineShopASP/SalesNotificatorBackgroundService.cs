using System.Diagnostics;

namespace OnlineShopASP
{
    public class SalesNotificatorBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SalesNotificatorBackgroundService> _logger;
        public SalesNotificatorBackgroundService(IServiceProvider serviceProvider, ILogger<SalesNotificatorBackgroundService> logger) 
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var localServiceProvider = scope.ServiceProvider;
            var emailSender = localServiceProvider.GetRequiredService<IEmailSender>();

            var users = new User[] 
            { 
                new User("address@example.com")
               
            };
           
            var sw = Stopwatch.StartNew();
            foreach (var user in users)
            {
                sw.Restart();
                _logger.LogInformation("Отправка сообщения на имеил {Email}",user.Email);

                try 
                { 
                    await emailSender.SendEmail(user.Email, "Промоакции", "Список акций");
                    _logger.LogInformation("Письмо отправлено {Email} за {ElapsedMilliseconds} мс", user.Email, sw.ElapsedMilliseconds);
                }
               catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при отправке сообщения на почту {Email}", user.Email);
                }

            }
        }
    }

    public record User(string Email);
}
