using System;

namespace Domo.Communication
{
    public class MessagingException : Exception
    {
        protected MessagingException(string message)
            : base(message)
        {
        }
    }
}