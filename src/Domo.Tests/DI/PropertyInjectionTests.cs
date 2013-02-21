using System;
using Domo.DI;
using Domo.DI.Construction;
using Domo.DI.Registration;
using NUnit.Framework;

namespace Domo.Tests.DI
{
    [TestFixture]
    public class PropertyInjectionTests : TestBase
    {
        [Test]
        public void PropertiesShouldBeInjected()
        {
            // Arrange
            var container = Container.Create(
                configuration =>
                configuration.
                    RegisterInstance(new ConstructionFactorySettings
                    {
                        UsePropertyInjection = true
                    }).
                    RegisterSingleton<Bar>().
                    RegisterSingleton<Foo>());

            // Act
            var bar = container.ServiceLocator.Resolve<Bar>();

            // Assert
            Assert.NotNull(bar);
            Assert.NotNull(bar.Foo);
        }

        [Test]
        public void PropertiesWithAttributeShouldBeInjected()
        {
            // Arrange
            var container = Container.Create(
                configuration =>
                configuration.
                    RegisterInstance(new ConstructionFactorySettings
                    {
                        PropertyInjectionAttribute = typeof(InjectedAttribute),
                        UsePropertyInjection = true
                    }).
                    RegisterSingleton<BarWithAttribute>().
                    RegisterSingleton<Foo>());

            // Act
            var bar = container.ServiceLocator.Resolve<BarWithAttribute>();

            // Assert
            Assert.NotNull(bar);
            Assert.NotNull(bar.Foo);
        }

        [Test]
        public void PropertiesShouldOnlyBeInjectedIfSpecified()
        {
            // Arrange
            var container = Container.Create(
                configuration =>
                configuration.
                    RegisterSingleton<Bar>().
                    RegisterSingleton<Foo>());

            // Act
            var bar = container.ServiceLocator.Resolve<Bar>();

            // Assert
            Assert.NotNull(bar);
            Assert.Null(bar.Foo);
        }

        [Test]
        public void PropertiesShouldOnlyBeInjectedIfCorrectAttributeIsSpecified()
        {
            // Arrange
            var container = Container.Create(
                configuration =>
                configuration.
                    RegisterInstance(new ConstructionFactorySettings
                    {
                        PropertyInjectionAttribute = typeof(InjectedAttribute),
                        UsePropertyInjection = true
                    }).
                    RegisterSingleton<Bar>().
                    RegisterSingleton<Foo>());

            // Act
            var bar = container.ServiceLocator.Resolve<Bar>();

            // Assert
            Assert.NotNull(bar);
            Assert.Null(bar.Foo);
        }

        private class Foo
        {
        }

        private class Bar
        {
            public Foo Foo { get; set; }
        }

        private class BarWithAttribute
        {
            [Injected]
            public Foo Foo { get; private set; }
        }

        private class InjectedAttribute : Attribute
        {
        }
    }
}