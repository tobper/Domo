using System;
using System.Threading.Tasks;
using Domo.Communication;
using Domo.DI;
using Moq;

namespace Domo.Tests.Communication
{
    public abstract class BusTests : UnitTests<IBus>
    {
        private Mock<IServiceLocator> _serviceLocator;

        protected override void CreateMocks()
        {
            _serviceLocator = new Mock<IServiceLocator>(MockBehavior.Strict);
        }

        protected override IBus CreateTestInstance()
        {
            return new Bus(_serviceLocator.Object);
        }

        protected void GivenAValidQueryHandlerHasBeenRegistered(int result)
        {
            _serviceLocator.
                Setup(l => l.TryResolve<IQueryHandler<DummyQuery, int>>(null)).
                Returns(new DummyQueryHandler(result));
        }

        protected void GivenNoCommandHandlerHasBeenRegistered()
        {
            _serviceLocator.
                Setup(l => l.TryResolve<ICommandHandler<DummyCommand>>(null)).
                Returns((ICommandHandler<DummyCommand>)null);
        }

        protected void GivenNoQueryHandlerHasBeenRegistered()
        {
            _serviceLocator.
                Setup(l => l.TryResolve<IQueryHandler<DummyQuery, int>>(null)).
                Returns((IQueryHandler<DummyQuery, int>)null);
        }

        protected class DummyCommand : ICommand
        {
            public Guid Id { get; set; }
        }

        protected class DummyQuery : IQuery
        {
            public Guid Id { get; set; }
        }

        private class DummyQueryHandler : IQueryHandler<DummyQuery, int>
        {
            private readonly int _result;

            public DummyQueryHandler(int result)
            {
                _result = result;
            }

            public Task<int> Handle(DummyQuery query)
            {
                return Task.FromResult(_result);
            }
        }
    }
}