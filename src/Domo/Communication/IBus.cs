namespace Domo.Communication
{
    public interface IBus
    {
        TResult Request<TQuery, TResult>(TQuery query) where TQuery : IQuery;
        void Post<TMessage>(TMessage message) where TMessage : IMessage;
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}