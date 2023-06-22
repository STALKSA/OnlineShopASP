using System.ComponentModel.DataAnnotations;

namespace OnlineShopASP
{
    public class SmtpConfig
    {
        [Required, RegularExpression(@"^[a-zA-Z0-9\.\-_]+@example\.com$", ErrorMessage = "Невалидный Host формат")] //хост в формате from_address@example.com
        public string Host { get; set; }
        [Required, RegularExpression(@"^[a-zA-Z0-9\.\-_]+$", ErrorMessage = "Невалидный UserName формат")]
        public string UserName { get; set; }
        [Required, RegularExpression(@"^[a-zA-Z0-9\.\-_]+$", ErrorMessage = "Невалидный Password формат")]
        public string Password { get; set; }
        [Required, Range(1, ushort.MaxValue, ErrorMessage = "Невалидное Port значение")]
        public int Port { get; set; }
        [Required, EmailAddress(ErrorMessage = "Невалидный FromEmail формат")]
        public string FromEmail { get; set; }
    }
}
