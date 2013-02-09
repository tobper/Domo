using Domo.DI;
using Domo.DI.Registration;
using NUnit.Framework;

namespace Domo.Tests.DI.Activation
{
    [TestFixture]
    public class InstanceActivatorTests
    {
        [Test]
        public void InstanceServiceShouldReturnSpecifiedInstance()
        {
            // Arrange
            var originalObject = new Foo();
            var container = Container.Create(c => c.RegisterInstance(originalObject));

            // Act
            var resolvedObject = container.ServiceLocator.Resolve<Foo>();

            // Assert
            Assert.AreSame(originalObject, resolvedObject);
        }

        private class Foo { }
    }
}