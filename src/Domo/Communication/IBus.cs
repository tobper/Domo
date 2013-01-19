using System.Threading.Tasks;

namespace Domo.Communication
{
    public interface IBus
    {
        Task<TResult> Request<TQuery, TResult>(TQuery query) where TQuery : IQuery;
        Task Post<TMessage>(TMessage message) where TMessage : IMessage;
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}