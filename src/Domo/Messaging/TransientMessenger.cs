using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domo.Messaging
{
    public class TransientMessenger : IMessenger
    {
        [ThreadStatic]
        private static Guid? _transactionId;

        private readonly IDomoServiceLocator _serviceLocator;

        public TransientMessenger(IDomoServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public Task Post<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            var handlers = _serviceLocator.TryResolveAll<IMessageHandler<TMessage>>();
            if (handlers.Length == 0)
                return Task.FromResult(0);

            if (message.TransactionId == Guid.Empty)
                message.TransactionId = GetTransactionId();

            var tasks = from handler in handlers
                        select CreateTask(handler.Handle, message, message.TransactionId);

            return Task.WhenAll(tasks);
        }

        public Task Send<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var handler = _serviceLocator.TryResolve<ICommandHandler<TCommand>>();
            if (handler == null)
                throw new SendCommandFailedException(typeof(TCommand));

            if (command.TransactionId == Guid.Empty)
                command.TransactionId = GetTransactionId();

            return CreateTask(handler.Handle, command, command.TransactionId);
        }

        private static Guid GetTransactionId()
        {
            return _transactionId ?? Guid.NewGuid();
        }

        private static Task CreateTask<T>(Action<T> method, T argument, Guid transactionId)
        {
            return Task.Run(() =>
            {
                var outerTransactionId = _transactionId;

                try
                {
                    _transactionId = transactionId;
                    method(argument);
                }
                finally
                {
                    _transactionId = outerTransactionId;
                }
            });
        }
    }
}
