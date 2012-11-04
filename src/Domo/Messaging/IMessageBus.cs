namespace Domo.Messaging
{
    public interface IMessageBus
    {
        void Post<TMessage>(TMessage message) where TMessage : IMessage;
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}