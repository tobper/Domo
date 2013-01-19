using NUnit.Framework;

namespace Domo.Tests.Communication
{
    [TestFixture]
    public class When_requesting_a_query_with_a_valid_query_handler : BusTests
    {
        private int _result;

        [Test]
        public void A_result_should_be_returned()
        {
            Assert.AreEqual(5, _result);
        }

        protected override void SetupPrerequisites()
        {
            GivenAValidQueryHandlerHasBeenRegistered(result: 5);
        }

        protected override void RunTest()
        {
            _result = TestInstance.Request<DummyQuery, int>(new DummyQuery());
        }
    }
}