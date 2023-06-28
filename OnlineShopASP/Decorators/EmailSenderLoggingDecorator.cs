using OnlineShopASP.Interfaces;

namespace OnlineShopASP.Decorators;

public class EmailSenderLoggingDecorator : IEmailSender  //перехват зависимости
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;


    public EmailSenderLoggingDecorator(IEmailSender emailSender, ILogger<EmailSenderLoggingDecorator> logger)
    {
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendEmailAsync(string recepientEmail, string subject, string body)
    {
        //перед отправкой мэйла
        _logger.LogInformation(message: $"Отправка на почту {recepientEmail}...{subject}, {body}");
        await _emailSender.SendEmailAsync(recepientEmail, subject, body);
    }
}
