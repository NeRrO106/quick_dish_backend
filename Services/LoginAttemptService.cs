using Microsoft.Extensions.Caching.Memory;

namespace QUickDish.API.Services
{
    public class LoginAttemptService
    {
        private readonly IMemoryCache _cache;
        private const int MaxAttempts = 5;
        private readonly TimeSpan BlockDuration = TimeSpan.FromMinutes(10);

        public LoginAttemptService(IMemoryCache cache)
        {
            _cache = cache;
        }


        public bool IsBlocked(string ip)
        {
            if (_cache.TryGetValue($"{ip}_blocked", out _))
                return true;

            return false;
        }

        public void LoginFailedAttempt(string ip)
        {
            int attempts = 0;

            if (_cache.TryGetValue($"{ip}_attempts", out int currentAttempts))
            {
                attempts = currentAttempts;
            }
            attempts++;

            if (attempts >= MaxAttempts)
            {
                _cache.Set($"{ip}_blocked", true, BlockDuration);
                _cache.Remove($"{ip}_attempts");
            }
            else
            {
                _cache.Set($"{ip}_attempts", attempts, TimeSpan.FromMinutes(15));
            }
        }

        public void ResetedAttempt(string ip)
        {
            _cache.Remove($"{ip}_attempts");
        }
    }
}
