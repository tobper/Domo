using System;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Moq;

namespace Domo.Tests.DI.Activation.ScopedServiceActivatorTests
{
    using ActivationContext = Domo.DI.Activation.ActivationContext;

    public class ScopedServiceActivatorTests : UnitTests<ScopedServiceActivator>
    {
        private ActivationContext _activationContext;

        private Type _activationType;

        protected Mock<IInstanceFactory> InstanceFactory { get; private set; }
        protected Mock<IServiceCache> SingletonCache { get; private set; }
        protected Mock<IServiceCache> ScopedCache { get; private set; }

        protected override void CreateMocks()
        {
            InstanceFactory = new Mock<IInstanceFactory>(MockBehavior.Strict);
            SingletonCache = new Mock<IServiceCache>(MockBehavior.Strict);
            ScopedCache = new Mock<IServiceCache>(MockBehavior.Strict);
        }

        protected override void SetupBehavior()
        {
            _activationType = typeof(Type);

            ScopedCache.
                Setup(c => c.Get(_activationType, InstanceFactory.Object, _activationContext)).
                Returns(null);
        }

        protected override ScopedServiceActivator CreateTestInstance()
        {
            return new ScopedServiceActivator(_activationType, InstanceFactory.Object);
        }

        protected override void PerformTest()
        {
            TestInstance.ActivateInstance(_activationContext);
        }

        protected void GivenAnActivationContextWithoutAScopedCache()
        {
            _activationContext = new ActivationContext(SingletonCache.Object, null);
        }

        protected void GivenAValidActivationContext()
        {
            _activationContext = new ActivationContext(SingletonCache.Object, ScopedCache.Object);
        }
    }
}