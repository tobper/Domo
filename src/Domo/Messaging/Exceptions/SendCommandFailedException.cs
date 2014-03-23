using System;

namespace Domo.Messaging
{
    public class SendCommandFailedException : MessagingException
    {
        public SendCommandFailedException(Type commandType)
            : base(CreateMessage(commandType))
        {
        }

        private static string CreateMessage(Type commandType)
        {
            return string.Format("No command handler for {0} has been registered.", commandType.Name);
        }
    }
}