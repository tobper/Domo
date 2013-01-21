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

        protected void GivenAValidQueryHandlerHasBeenRegistered(string name)
        {
            _serviceLocator.
                Setup(l => l.TryResolve<IQueryHandler<Dummy, IQuery>>(null)).
                Returns(new DummyQueryHandler(name));
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
                Setup(l => l.TryResolve<IQueryHandler<Dummy, IQuery>>(null)).
                Returns((IQueryHandler<Dummy, IQuery>)null);
        }

        protected class DummyCommand : ICommand
        {
            public Guid TransactionId { get; set; }
            public string Name { get; set; }
        }

        protected class Dummy
        {
            public string Name { get; set; }
        }

        private class DummyQueryHandler : IQueryHandler<Dummy, IQuery>
        {
            private readonly string _name;

            public DummyQueryHandler(string name)
            {
                _name = name;
            }

            public async Task<Dummy> Handle(IQuery query)
            {
                return new Dummy
                {
                    Name = _name
                };
            }
        }
    }
}