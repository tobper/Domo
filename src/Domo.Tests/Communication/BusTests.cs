using System;
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

        protected void GivenNoCommandHandlerHasBeenRegistered()
        {
            _serviceLocator.
                Setup(l => l.TryResolve<ICommandHandler<DummyCommand>>(null)).
                Returns((ICommandHandler<DummyCommand>)null);
        }

        protected class DummyCommand : ICommand
        {
            public Guid TransactionId { get; set; }
            public string Name { get; set; }
        }
    }
}