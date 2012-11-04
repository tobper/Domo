using System.Linq;
using Domo.DI;
using NUnit.Framework;

namespace Domo.Tests.DI.ServiceContainerTests
{
    [TestFixture]
    public class When_creating_an_empty_container : ServiceContainerTests
    {
        [Test]
        public void The_service_container_should_automatically_be_registered()
        {
            var serviceContainerType = typeof(IServiceContainer);
            var serviceContainer = TestInstance.Resolve(serviceContainerType);

            Assert.IsTrue(serviceContainer.Any());
        }

        [Test]
        public void The_service_locator_should_automatically_be_registered()
        {
            var serviceLocatoreType = typeof(IServiceLocator);
            var serviceLocator = TestInstance.Resolve(serviceLocatoreType);

            Assert.IsTrue(serviceLocator.Any());
        }

        protected override void SetupGivens()
        {
            GivenNoRegistrationIsBeingUsed();
        }
    }
}
