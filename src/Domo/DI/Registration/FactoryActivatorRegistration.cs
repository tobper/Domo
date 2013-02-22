using System;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public class FactoryActivatorRegistration<TService> :
        IFactoryActivatorRegistration<TService>,
        IFactoryActivatorRegistration,
        IActivatorConfiguration
    {
        public ServiceIdentity Identity { get; private set; }
        public Type ConcreteType { get; set; }
        public IFactory Factory { get; set; }
        public IServiceScope ServiceScope { get; set; }

        public FactoryActivatorRegistration(Type serviceType, IServiceScope serviceScope)
        {
            Identity = new ServiceIdentity(serviceType);
            ServiceScope = serviceScope;
        }

        public FactoryActivatorRegistration(ServiceIdentity identity, IServiceScope serviceScope)
        {
            Identity = identity;
            ServiceScope = serviceScope;
        }

        public FactoryActivatorRegistration(IFluentRegistration fluentRegistration, IServiceScope serviceScope)
        {
            Identity = fluentRegistration.Identity;
            ServiceScope = serviceScope;

            fluentRegistration.Using(this);
        }

        public FactoryActivatorRegistration(IFluentRegistration<TService> fluentRegistration, IServiceScope serviceScope)
        {
            Identity = fluentRegistration.Identity;
            ServiceScope = serviceScope;

            fluentRegistration.Using(this);
        }

        IFactoryActivatorRegistration IFactoryActivatorRegistration.UsingConcreteType(Type concreteType)
        {
            UsingConcreteType(concreteType);

            return this;
        }

        IFactoryActivatorRegistration IFactoryActivatorRegistration.UsingConcreteType<TConcrete>()
        {
            var concreteType = typeof(TConcrete);

            UsingConcreteType(concreteType);

            return this;
        }

        IFactoryActivatorRegistration IFactoryActivatorRegistration.UsingFactory(Func<object> factoryDelegate)
        {
            var delegateFactory = new DelegateFactory(factoryDelegate);

            UsingFactory(delegateFactory);

            return this;
        }

        IFactoryActivatorRegistration IFactoryActivatorRegistration.UsingFactory(Func<IInjectionContext, object> factoryDelegate)
        {
            var delegateFactory = new DelegateFactory(factoryDelegate);

            UsingFactory(delegateFactory);

            return this;
        }

        IFactoryActivatorRegistration IFactoryActivatorRegistration.UsingFactory(IFactory factory)
        {
            UsingFactory(factory);

            return this;
        }

        public IFactoryActivatorRegistration<TService> UsingConcreteType(Type concreteType)
        {
            ConcreteType = concreteType;

            return this;
        }

        public IFactoryActivatorRegistration<TService> UsingConcreteType<TConcrete>() where TConcrete : TService
        {
            var concreteType = typeof(TConcrete);

            return UsingConcreteType(concreteType);
        }

        public IFactoryActivatorRegistration<TService> UsingFactory(Func<TService> factoryDelegate)
        {
            var delegateFactory = new DelegateFactory<TService>(factoryDelegate);

            UsingFactory(delegateFactory);

            return this;
        }

        public IFactoryActivatorRegistration<TService> UsingFactory(Func<IInjectionContext, TService> factoryDelegate)
        {
            var delegateFactory = new DelegateFactory<TService>(factoryDelegate);

            UsingFactory(delegateFactory);

            return this;
        }

        public IFactoryActivatorRegistration<TService> UsingFactory(IFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            Factory = factory;

            return this;
        }

        public IActivator GetService(IContainer container)
        {
            var factory = Factory ?? new ConstructionFactory(ConcreteType ?? Identity.ServiceType);
            var service = new FactoryActivator(Identity, factory, ServiceScope);

            return service;
        }
    }
}