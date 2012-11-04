using System.Threading.Tasks;
using Domo.DI;

namespace Domo.Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly IServiceLocator _serviceLocator;

        public MessageBus(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public void Post<TMessage>(TMessage message) where TMessage : IMessage
        {
            var handlers = _serviceLocator.ResolveAll<IMessageHandler<TMessage>>();
            if (handlers.Length == 0)
                return;

            Task.Run(() =>
            {
                foreach (var handler in handlers)
                {
                    handler.Handle(message);
                }
            });
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _serviceLocator.Resolve<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }
    }
}
