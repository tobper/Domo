using System.Linq;
using System.Threading.Tasks;
using Domo.DI;

namespace Domo.Communication
{
    public class TransientBus : IBus
    {
        private readonly IServiceLocator _serviceLocator;

        public TransientBus(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public Task Post<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            var handlers = _serviceLocator.TryResolveAll<IMessageHandler<TMessage>>();

            return Task.WhenAll(
                from handler in handlers
                select handler.Handle(message));
        }

        public Task Send<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var handler = _serviceLocator.TryResolve<ICommandHandler<TCommand>>();
            if (handler == null)
                throw new SendCommandFailedException(typeof(TCommand));

            return handler.Handle(command);
        }
    }
}
