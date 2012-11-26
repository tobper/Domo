using System;
using Domo.DI;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Domo.DI.Redirection;
using Moq;
using ActivationContext = Domo.DI.Activation.ActivationContext;

namespace Domo.Tests.DI.Activation.SingletonFactoryActivatorTests
{
    public class SingletonFactoryActivatorTests : UnitTests<SingletonFactoryActivator>
    {
        private ActivationContext _activationContext;

        private string _serviceName;
        private Type _serviceType;
        private Type _redirectionType;
        private object _activatedInstance;

        protected Mock<IContainer> Container { get; private set; }
        protected Mock<IFactoryManager> FactoryManager { get; private set; }
        protected Mock<IInstanceCache> SingletonCache { get; private set; }
        protected Mock<ITypeRedirector> TypeRedirector { get; private set; }

        protected override void CreateMocks()
        {
            Container = new Mock<IContainer>(MockBehavior.Strict);
            FactoryManager = new Mock<IFactoryManager>(MockBehavior.Strict);
            SingletonCache = new Mock<IInstanceCache>(MockBehavior.Strict);
            TypeRedirector = new Mock<ITypeRedirector>(MockBehavior.Strict);

            _serviceName = null;
            _serviceType = typeof(Type);
            _redirectionType = null;
        }

        protected override void SetupBehavior()
        {
            SingletonCache.
                Setup(c => c.Get(_serviceType, null, It.IsAny<Func<object>>())).
                Returns(_activatedInstance);

            TypeRedirector.
                Setup(c => c.GetRedirection(_serviceType, _serviceName)).
                Returns(_redirectionType);
        }

        protected override SingletonFactoryActivator CreateTestInstance()
        {
            return new SingletonFactoryActivator(FactoryManager.Object, SingletonCache.Object, TypeRedirector.Object);
        }

        protected override void RunTest()
        {
            TestInstance.ActivateInstance(_activationContext, _serviceType, _serviceName);
        }

        protected void GivenAValidActivationContext()
        {
            _activationContext = new ActivationContext(Container.Object);
        }

        protected void GivenTheCacheReturnsAnInstance()
        {
            _activatedInstance = GetType();
        }
    }
}