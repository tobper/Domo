using System;

namespace Domo.Messaging
{
    public class MessagingException : Exception
    {
        protected MessagingException(string message)
            : base(message)
        {
        }
    }
}