namespace Domo.Messaging
{
    public class SendCommandFailedException : MessagingException
    {
        public SendCommandFailedException(string message)
            : base(message)
        {
        }
    }
}