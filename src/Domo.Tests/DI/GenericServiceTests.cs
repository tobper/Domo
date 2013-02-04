using Domo.DI.Registration;
using Domo.DI.Registration.Conventions;
using NUnit.Framework;

namespace Domo.Tests.DI.Container
{
    public interface IFoo<T>
    { }

    public class HomeFoo : IFoo<int>
    { }

    public class GenericServiceTests : ContainerTests
    {
        [Test]
        public void ResolveAllServices()
        {
            var instances = TestInstance.ServiceLocator.ResolveAll<IFoo<int>>();

            Assert.IsNotNull(instances);
            Assert.AreEqual(1, instances.Length);
        }

        [Test]
        public void ResolveDefaultService()
        {
            var instance = TestInstance.ServiceLocator.Resolve<IFoo<int>>();

            Assert.IsNotNull(instance);
        }

        [Test]
        public void ResolveNamedService()
        {
            var instance = TestInstance.ServiceLocator.Resolve<IFoo<int>>("Home");

            Assert.IsNotNull(instance);
        }

        protected override void SetupBehavior()
        {
            GivenThatAnAssemblyScannerIsBeingUsed(Scan);
        }

        private void Scan(IAssemblyScanner scanner)
        {
            scanner.
                UseBasicConventions().
                ScanAssemblyContaining<ContainerTests>();
        }
    }
}