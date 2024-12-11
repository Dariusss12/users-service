using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace users_service.Src.Helpers
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}