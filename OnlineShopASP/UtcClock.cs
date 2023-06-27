using OnlineShopASP.Interfaces;

namespace OnlineShopASP
{
    public class UtcClock : IClock
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
