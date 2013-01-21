using System;
using System.Linq;
using System.Threading.Tasks;
using Domo.DI;

namespace Domo.Communication
{
    public class Bus : IBus
    {
        private readonly IServiceLocator _serviceLocator;

        public Bus(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public Task<TResult> Request<TResult>()
        {
            var query = new EmptyQuery();
            var result = Request<TResult, IQuery>(query);

            return result;
        }

        public Task<TResult> Request<TResult, TQuery>(TQuery query) where TQuery : IQuery
        {
            var handler = _serviceLocator.TryResolve<IQueryHandler<TResult, TQuery>>();
            if (handler == null)
                throw new RequestQueryFailedException(typeof(TQuery), typeof(TResult));

            return handler.Handle(query);
        }

        public Task Post<TMessage>(TMessage message) where TMessage : IMessage
        {
            var handlers = _serviceLocator.ResolveAll<IMessageHandler<TMessage>>();
            if (handlers.Length == 0)
                return null;

            return Task.WhenAll(
                from handler in handlers
                select handler.Handle(message));
        }

        public Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _serviceLocator.TryResolve<ICommandHandler<TCommand>>();
            if (handler == null)
                throw new SendCommandFailedException(typeof(TCommand));

            return handler.Handle(command);
        }

        private class EmptyQuery : IQuery
        {
            public Guid TransactionId { get; set; }

            public EmptyQuery()
            {
                TransactionId = Guid.NewGuid();
            }
        }
    }
}
