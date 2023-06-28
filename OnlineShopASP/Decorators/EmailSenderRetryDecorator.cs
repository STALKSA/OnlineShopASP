using OnlineShopASP.Interfaces;
using Polly;
using Polly.Retry;

namespace OnlineShopASP.Decorators
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

        public async Task SendEmailAsync(string recepientEmail, string subject, string body)
        {

            AsyncRetryPolicy? policy = Policy
                  .Handle<Exception>()
                  .WaitAndRetryAsync(new[]
                  {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                  }, (exception, retryAttempt) =>
                  {
                      _logger.LogWarning(exception, "Ошибка при отправке сообщения. Повторная попытка: {Attempt}", retryAttempt);
                  });

                var result = await policy.ExecuteAndCaptureAsync(
                           () => _emailSenderImplementation.SendEmailAsync(recepientEmail, subject, body));

                if (result.Outcome == OutcomeType.Failure)
                {
                    _logger.LogError(result.FinalException, "Ошибка при отправке сообщения");
                }

        }
    }
}
