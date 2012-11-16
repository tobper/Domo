using Domo.DI;
using NUnit.Framework;

namespace Domo.Tests.DI.ServiceContainerTests
{
    [TestFixture]
    public class When_creating_a_container : ServiceContainerTests
    {
        [Test]
        public void The_container_should_automatically_be_registered()
        {
            var containerType = typeof(IContainer);
            var container = TestInstance.Resolve(containerType, null);

            Assert.IsNotNull(container);
        }

        [Test]
        public void The_service_locator_should_automatically_be_registered()
        {
            var serviceLocatoreType = typeof(IServiceLocator);
            var serviceLocator = TestInstance.Resolve(serviceLocatoreType, null);

            Assert.IsNotNull(serviceLocator);
        }

        protected override void SetupPrerequisites()
        {
            GivenNoRegistrationIsBeingUsed();
        }
    }
}
