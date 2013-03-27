namespace Domo.Communication
{
    public interface IMessageHandler<in TMessage>
        where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}