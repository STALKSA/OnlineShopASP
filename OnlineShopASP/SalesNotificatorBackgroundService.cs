using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace OnlineShopASP
{
    public class SalesNotificatorBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SalesNotificatorBackgroundService> _logger;
        private readonly int _attemptsLimit;
        public SalesNotificatorBackgroundService(IServiceProvider serviceProvider, 
            ILogger<SalesNotificatorBackgroundService> logger,
            IConfiguration configuration) 
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _attemptsLimit = configuration.GetValue<int>("SalesEmailAttemptsCount");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var localServiceProvider = scope.ServiceProvider;
            var emailSender = localServiceProvider.GetRequiredService<IEmailSender>();

            var users = new User[] 
            { 
                new User("ksy9090@mail.ru")
               
            };
           
            var sw = Stopwatch.StartNew();
            foreach (var user in users)
            {
                sw.Restart();
                _logger.LogInformation("Отправка сообщения на имеил {Email}",user.Email);

                for (var attempt = 1; attempt <= _attemptsLimit; attempt++)
                {
                    try
                    {
                        await emailSender.SendEmail(user.Email, "Промоакции", "Список акций");
                        _logger.LogInformation("Письмо отправлено {Email} за {ElapsedMilliseconds} мс", user.Email, sw.ElapsedMilliseconds);
                        break;
                    }
                    catch (Exception ex) when (attempt < _attemptsLimit)
                    { 
                         _logger.LogWarning(ex, "Повторная отправка сообщения на почту {Email}, попытка {counter}", user.Email, attempt + 1);
                        await Task.Delay(1000, stoppingToken); 
                    }
                    catch(Exception ex)
                    {
                         _logger.LogError(ex, "Ошибка при отправке сообщения на почту {Email}", user.Email);
                        await Task.Delay(1000, stoppingToken);
                    } 
                
                }  
            
            }
        }
    }

    public record User(string Email);
}
