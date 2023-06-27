namespace OnlineShopASP.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string recepientEmail, string subject, string body);
    }
}