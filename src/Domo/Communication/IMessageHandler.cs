using System.Threading.Tasks;

namespace Domo.Communication
{
    public interface IMessageHandler<in TMessage>
        where TMessage : IMessage
    {
        Task Handle(TMessage message);
    }
}