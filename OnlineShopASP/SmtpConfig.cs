using System.ComponentModel.DataAnnotations;

namespace OnlineShopASP
{
    public class SmtpConfig
    {
        [Required, RegularExpression(@"^[a-zA-Z0-9\.\-_]+@example\.com$")] //хост в формате from_address@example.com
        public string Host { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Range(1, ushort.MaxValue)]
        public int Port { get; set; }
        [Required, EmailAddress]
        public string FromEmail { get; set; }
    }
}
