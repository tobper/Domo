using Domo.DI;
using Domo.DI.Registration;
using Domo.Tests.DI.TestData;
using NUnit.Framework;

namespace Domo.Tests.DI
{
    [TestFixture]
    public class AssemblyScanningTests
    {
        [Test]
        public void ServicesShouldBeAutomaticallyRegistered()
        {
            // Arrange
            var container = Container.Create(
                configuration =>
                configuration.Scan(
                    scanner =>
                    scanner.
                        UseBasicConventions().
                        ScanAssemblyContaining<ContainerTests>()));

            // Act
            var foo = container.ServiceLocator.Resolve<IFoo>();

            // Assert
            Assert.NotNull(foo);
        }
    }
}