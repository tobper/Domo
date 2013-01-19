using System;
using Domo.Communication;
using NUnit.Framework;

namespace Domo.Tests.Communication
{
    [TestFixture]
    public class When_sending_a_command_with_no_registered_command_handler : BusTests
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