namespace Domo.Communication
{
    public interface IMessageHandler<in TCommand> where TCommand : IMessage
    {
        void Handle(TCommand command);
    }
}