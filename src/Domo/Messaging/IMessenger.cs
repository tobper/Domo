using System.Threading.Tasks;

namespace Domo.Messaging
{
    public interface IMessenger
    {
        Task Post<TMessage>(TMessage message) where TMessage : IMessage;
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}