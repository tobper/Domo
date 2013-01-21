using NUnit.Framework;

namespace Domo.Tests.Communication
{
    [TestFixture]
    public class When_requesting_a_query_with_a_valid_query_handler : BusTests
    {
        private Dummy _result;

        [Test]
        public void A_result_should_be_returned()
        {
            Assert.IsNotNull(_result);
            Assert.AreEqual("John Doe", _result.Name);
        }

        protected override void SetupPrerequisites()
        {
            GivenAValidQueryHandlerHasBeenRegistered(name: "John Doe");
        }

        protected override async void RunTest()
        {
            _result = await TestInstance.Request<Dummy>();
        }
    }
}