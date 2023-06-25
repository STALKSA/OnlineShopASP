using System.ComponentModel.DataAnnotations;

namespace OnlineShopASP
{
    public class SmtpConfig
    {
        [Required, RegularExpression(@"[^@\s]+\.[^@\s]+\.[^@\s]+$")] 
        public string Host { get; set; }
        
        [Required] 
        public string UserName { get; set; }
       
        [Required]
        public string Password { get; set; }
       
        [Required, Range(1, ushort.MaxValue)]
        public int Port { get; set; }
        
        [Required]
        public string FromEmail { get; set; }
    }
}
