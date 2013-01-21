using System;
using Domo.Communication;
using NUnit.Framework;

namespace Domo.Tests.Communication
{
    [TestFixture]
    public class When_requesting_a_query_with_no_registered_query_handler : BusTests
    {
        [Test]
        public void An_exception_should_be_thrown()
        {
        }

        protected override Type GetExpectedException()
        {
            return typeof(RequestQueryFailedException);
        }

        protected override void SetupPrerequisites()
        {
            GivenNoQueryHandlerHasBeenRegistered();
        }

        protected override void RunTest()
        {
            TestInstance.Request<Dummy>();
        }
    }
}