using System;
using Domo.DI;
using Domo.Messaging;
using Moq;

namespace Domo.Tests.Messaging
{
    public abstract class MessageBusTests : UnitTests<IMessageBus>
    {
        private Mock<IServiceLocator> _serviceLocator;

        protected override void CreateMocks()
        {
            _serviceLocator = new Mock<IServiceLocator>(MockBehavior.Strict);
        }

        protected override IMessageBus CreateTestInstance()
        {
            return new MessageBus(_serviceLocator.Object);
        }

        protected void GivenNoCommandHandlerHasBeenRegistered()
        {
            _serviceLocator.
                Setup(l => l.TryResolve<ICommandHandler<DummyCommand>>()).
                Returns((ICommandHandler<DummyCommand>)null);
        }

        protected class DummyCommand : ICommand
        {
            public Guid Id { get; set; }
        }
    }
}