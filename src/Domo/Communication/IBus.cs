using System.Threading.Tasks;

namespace Domo.Communication
{
    public interface IBus
    {
        Task<TResult> Request<TResult>();
        Task<TResult> Request<TResult, TQuery>(TQuery query) where TQuery : IQuery;
        Task Post<TMessage>(TMessage message) where TMessage : IMessage;
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}