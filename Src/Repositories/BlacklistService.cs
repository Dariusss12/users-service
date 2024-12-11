using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using users_service.Src.Repositories.Interfaces;

namespace users_service.Src.Repositories
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