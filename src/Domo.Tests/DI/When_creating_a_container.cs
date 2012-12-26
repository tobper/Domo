using Domo.DI;
using NUnit.Framework;

namespace Domo.Tests.DI
{
    [TestFixture]
    public class When_creating_a_container : ContainerTests
    {
        [Test]
        public void The_container_should_automatically_be_registered()
        {
            var container = Resolve<IContainer>();

            Assert.IsNotNull(container);
        }

        [Test]
        public void The_service_locator_should_automatically_be_registered()
        {
            var serviceLocator = Resolve<IServiceLocator>();

            Assert.IsNotNull(serviceLocator);
        }

        protected override void SetupPrerequisites()
        {
            GivenNoRegistrationIsBeingUsed();
        }

        private object Resolve<T>()
        {
            var serviceType = typeof(T);
            var identity = new ServiceIdentity(serviceType);
            var instance = TestInstance.Resolve(identity);

            return instance;
        }
    }
}
