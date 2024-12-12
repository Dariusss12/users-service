using users_service.Src.Services.Interfaces;

namespace users_service.Src.Services
{
    public class BlacklistService : IBlacklistService
    {
        private readonly HashSet<string> _blacklist = [];

        public void AddToBlacklist(string token)
        {
           _blacklist.Add(token);
        }

        public bool IsBlacklisted(string token)
        {
            return _blacklist.Contains(token);
        }
    }
}