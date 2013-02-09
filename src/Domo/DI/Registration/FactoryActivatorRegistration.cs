using System;
using Domo.DI.Activation;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public class FactoryActivatorRegistration<TService> :
        IFactoryActivatorRegistration<TService>,
        IFactoryActivatorRegistration,
        IActivatorConfiguration
    {
        public ServiceIdentity Identity { get; private set; }
        public ActivationScope ActivationScope { get; set; }
        public Type ConcreteType { get; set; }
        public IFactory Factory { get; set; }

        public FactoryActivatorRegistration(Type serviceType, ActivationScope scope)
        {
            Identity = new ServiceIdentity(serviceType);
            ActivationScope = scope;
        }

        public FactoryActivatorRegistration(ServiceIdentity identity, ActivationScope scope)
        {
            Identity = identity;
            ActivationScope = scope;
        }

        public FactoryActivatorRegistration(IFluentRegistration fluentRegistration, ActivationScope scope)
        {
            Identity = fluentRegistration.Identity;
            ActivationScope = scope;

            fluentRegistration.Using(this);
        }

        public FactoryActivatorRegistration(IFluentRegistration<TService> fluentRegistration, ActivationScope scope)
        {
            Identity = fluentRegistration.Identity;
            ActivationScope = scope;

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
            var service = CreateService(factory);

            return service;
        }

        private IActivator CreateService(IFactory factory)
        {
            switch (ActivationScope)
            {
                case ActivationScope.Singleton:
                    return new SingletonFactoryActivator(Identity, factory);

                case ActivationScope.Transient:
                    return new TransientFactoryActivator(Identity, factory);

                default:
                    throw new InvalidScopeException(ActivationScope, Identity.ServiceType);
            }
        }
    }
}