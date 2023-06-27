
using Polly;
using Polly.Retry;

namespace OnlineShopASP
{
    public class EmailSenderRetryDecorator : IEmailSender
    {
        private readonly IEmailSender _emailSenderImplementation;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly int _attemptsLimit;


        public EmailSenderRetryDecorator(IEmailSender emailSenderImplementation, 
            ILogger<EmailSenderRetryDecorator> logger,
            IConfiguration configuration)
        {
            _emailSenderImplementation = emailSenderImplementation ?? throw new ArgumentNullException(nameof(emailSenderImplementation));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _attemptsLimit = configuration.GetValue<int>("SalesEmailAttemptsCount");
        }

        public async Task SendEmail(string recepientEmail, string subject, string body)
        {

            try
            {
                RetryPolicy? policy = Policy
                  .Handle<Exception>()
                  .WaitAndRetry(new[]
                  {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                  }, (exception, retryAttempt) =>
                  {
                      _logger.LogWarning(exception, "Ошибка при отправке сообщения. Повторная попытка: {Attempt}", retryAttempt);
                  });

                var result = policy.ExecuteAndCapture(
                           () => _emailSenderImplementation.SendEmail(recepientEmail, subject, body));

                if (result.Outcome == OutcomeType.Failure)
                {
                    _logger.LogError(result.FinalException, "Ошибка при отправке сообщения");
                }

            }

            catch (Exception ex)
               {
                _logger.LogError(ex, "Ошибка Подключения...");
            }
             
        }
    }
}
