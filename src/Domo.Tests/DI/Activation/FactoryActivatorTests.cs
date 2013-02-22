using Domo.DI;
using Domo.DI.Activation;
using Domo.DI.Construction;
using Domo.DI.Registration;
using Moq;
using NUnit.Framework;

namespace Domo.Tests.DI.Activation
{
    [TestFixture]
    public class FactoryActivatorTests : TestBase
    {
        [Test]
        public void SingletonServicesShouldBeCached()
        {
            // Arrange
            var container = Container.Create(c => c.RegisterSingleton<Foo>());

            // Act
            var resolvedObject1 = container.ServiceLocator.Resolve<Foo>();
            var resolvedObject2 = container.ServiceLocator.Resolve<Foo>();

            // Assert
            Assert.NotNull(resolvedObject1);
            Assert.NotNull(resolvedObject2);
            Assert.AreSame(resolvedObject1, resolvedObject2);
        }

        [Test]
        public void TransientServicesShouldNotBeCached()
        {
            // Arrange
            var container = Container.Create(c => c.RegisterTransient<Foo>());

            // Act
            var resolvedObject1 = container.ServiceLocator.Resolve<Foo>();
            var resolvedObject2 = container.ServiceLocator.Resolve<Foo>();

            // Assert
            Assert.NotNull(resolvedObject1);
            Assert.NotNull(resolvedObject2);
            Assert.AreNotSame(resolvedObject1, resolvedObject2);
        }

        [Test]
        public void FactoryDelegateShouldBeCalledWhenCreatingService()
        {
            // Arrange
            var originalObject = new Foo();
            var container = Container.Create(c => c.
                Register<Foo>().
                AsTransient().
                UsingFactory(() => originalObject));

            // Act
            var resolvedObject = container.ServiceLocator.Resolve<Foo>();

            // Assert
            Assert.AreSame(originalObject, resolvedObject);
        }

        [Test]
        public void FactoryShouldBeCalledWhenCreatingService()
        {
            // Arrange
            var originalObject = new Foo();
            var factoryMock = CreateMock<IFactory>(m => m.
                Setup(f => f.CreateService(It.IsAny<IInjectionContext>())).
                Returns(originalObject));

            var container = Container.Create(c => c.
                Register<Foo>().
                AsTransient().
                UsingFactory(factoryMock.Object));

            // Act
            var resolvedObject = container.ServiceLocator.Resolve<Foo>();

            // Assert
            Assert.AreSame(originalObject, resolvedObject);
        }

        private class Foo { }
    }
}