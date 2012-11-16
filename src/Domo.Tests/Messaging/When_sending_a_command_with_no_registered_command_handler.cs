using System;
using Domo.Messaging;
using NUnit.Framework;

namespace Domo.Tests.Messaging
{
    [TestFixture]
    public class When_sending_a_command_with_no_registered_command_handler : MessageBusTests
    {
        [Test]
        public void An_exception_should_be_thrown()
        {
        }

        protected override Type GetExpectedException()
        {
            return typeof(SendCommandFailedException);
        }

        protected override void SetupPrerequisites()
        {
            GivenNoCommandHandlerHasBeenRegistered();
        }

        protected override void RunTest()
        {
            TestInstance.Send(new DummyCommand());
        }
    }
}