using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public class TokenToBlacklistMessage
    {
        public string Token { get; set; } = string.Empty;
    }
}