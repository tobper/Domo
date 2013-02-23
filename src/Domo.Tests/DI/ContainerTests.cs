using Domo.DI;
using Domo.DI.Caching;
using NUnit.Framework;

namespace Domo.Tests.DI
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void ServiceLocatorShouldAutomaticallyBeRegistered()
        {
            // Arrange
            var container = Container.Create();

            // Act
            var resolvedServiceLocator = container.ServiceLocator.Resolve<IServiceLocator>();

            // Assert
            Assert.NotNull(resolvedServiceLocator);
        }

        [Test]
        public void ContainerShouldAutomaticallyBeRegistered()
        {
            // Arrange
            var container = Container.Create();

            // Act
            var resolvedContainer = container.ServiceLocator.Resolve<IContainer>();

            // Assert
            Assert.NotNull(resolvedContainer);
        }

        [Test]
        public void SingletonCacheShouldAutomaticallyBeRegistered()
        {
            // Arrange
            var container = Container.Create();

            // Act
            var resolvedCache = container.ServiceLocator.Resolve<IServiceCache>();

            // Assert
            Assert.NotNull(resolvedCache);
        }
    }
}