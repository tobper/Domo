using System;
using Domo.DI;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Moq;

namespace Domo.Tests.DI.Activation
{
    public class SingletonActivatorTests : UnitTests<SingletonActivator>
    {
        private IInjectionContext _context;

        private ServiceIdentity _serviceIdentity;
        private ServiceIdentity _realServiceIdentity;

        protected Mock<IContainer> Container { get; private set; }
        protected Mock<IFactoryContainer> FactoryManager { get; private set; }
        protected Mock<IInstanceCache> SingletonCache { get; private set; }
        protected Mock<IIdentityManager> TypeRedirector { get; private set; }

        protected override void CreateMocks()
        {
            Container = new Mock<IContainer>(MockBehavior.Strict);
            FactoryManager = new Mock<IFactoryContainer>(MockBehavior.Strict);
            SingletonCache = new Mock<IInstanceCache>(MockBehavior.Strict);
            TypeRedirector = new Mock<IIdentityManager>(MockBehavior.Strict);

            _serviceIdentity = new ServiceIdentity(typeof(Type));
            _realServiceIdentity = null;
        }

        protected override void SetupBehavior()
        {
            TypeRedirector.
                Setup(c => c.GetRealIdentity(_serviceIdentity)).
                Returns(_realServiceIdentity);
        }

        protected override SingletonActivator CreateTestInstance()
        {
            return new SingletonActivator(FactoryManager.Object, TypeRedirector.Object, SingletonCache.Object);
        }

        protected override void RunTest()
        {
            TestInstance.ActivateService(_context, _serviceIdentity);
        }

        protected void GivenAValidActivationContext()
        {
            _context = new InjectionContext(Container.Object);
        }

        protected void GivenTheSingletonCacheReturnsAnExistingInstance()
        {
            SingletonCache.
                Setup(c => c.Get(_serviceIdentity, It.IsAny<Func<object>>())).
                Returns(new object());
        }

        protected void GivenTheSingletonCacheAsksTheFactoryDelegateForAnInstance()
        {
            SingletonCache.
                Setup(c => c.Get(_serviceIdentity, It.IsAny<Func<object>>())).
                Returns<ServiceIdentity, Func<object>>((_, factoryDelegate) => factoryDelegate());
        }
    }
}