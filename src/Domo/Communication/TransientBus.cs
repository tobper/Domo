using System;
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
            var tasks = from handler in handlers
                        select CreateTask(handler.Handle, message);

            return Task.WhenAll(tasks);
        }

        public Task Send<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var handler = _serviceLocator.TryResolve<ICommandHandler<TCommand>>();
            if (handler == null)
                throw new SendCommandFailedException(typeof(TCommand));

            return CreateTask(handler.Handle, command);
        }

        private static Task CreateTask<T>(Action<T> method, T argument)
        {
            return Task.Run(() => method(argument));
        }
    }
}
