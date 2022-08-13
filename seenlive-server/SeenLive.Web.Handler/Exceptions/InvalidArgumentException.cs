using System;

namespace SeenLive.Web.Handler.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string? message) : base(message)
        {
        }
    }
}
