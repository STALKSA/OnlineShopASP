namespace OnlineShopASP
{
    public interface IEmailSender
    {
       Task SendEmail(string recepientEmail, string subject, string body);
    }
}