using System;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Creation;

namespace Domo.DI.Registration
{
    public class TypeRegistration : ITypeRegistration
    {
        private readonly IContainer _container;
        private readonly IFactoryContainer _factoryContainer;
        private readonly IInstanceCache _singletonInstanceCache;
        private readonly ITypeSubstitution _typeSubstitution;

        public LifeStyle DefaultLifeStyle { get; private set; }

        public TypeRegistration(
            IContainer container,
            IFactoryContainer factoryContainer,
            IInstanceCache singletonInstanceCache,
            ITypeSubstitution typeSubstitution)
        {
            _container = container;
            _factoryContainer = factoryContainer;
            _singletonInstanceCache = singletonInstanceCache;
            _typeSubstitution = typeSubstitution;

            DefaultLifeStyle = LifeStyle.Singleton;
        }

        public ITypeRegistration SetDefaultLifeStyle(LifeStyle lifeStyle)
        {
            if (lifeStyle == LifeStyle.Default)
                throw new InvalidLifeStyleException(lifeStyle);

            DefaultLifeStyle = lifeStyle;

            return this;
        }

        public ITypeRegistration RegisterDelegate<TService>(Func<IInjectionContext, TService> factoryDelegate, string serviceName, LifeStyle lifeStyle)
        {
            if (factoryDelegate == null)
                throw new ArgumentNullException("factoryDelegate");

            var serviceType = typeof(TService);
            var activatorType = GetActivatorType(lifeStyle, serviceType);
            var factory = new DelegateFactory<TService>(factoryDelegate);
            var identity = new ServiceIdentity(serviceType, serviceName);

            _factoryContainer.AddFactory(serviceType, factory);
            _container.Register(identity, activatorType);

            return this;
        }

        public ITypeRegistration RegisterDelegate(Func<IInjectionContext, object> factoryDelegate, ServiceIdentity identity, LifeStyle lifeStyle)
        {
            if (factoryDelegate == null)
                throw new ArgumentNullException("factoryDelegate");

            var activatorType = GetActivatorType(lifeStyle, identity.ServiceType);
            var factory = new DelegateFactory(factoryDelegate);

            _factoryContainer.AddFactory(identity.ServiceType, factory);
            _container.Register(identity, activatorType);

            return this;
        }

        public ITypeRegistration RegisterSingleton<TService>(TService instance, string serviceName) where TService : class
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            var serviceType = typeof(TService);
            var identity = new ServiceIdentity(serviceType, serviceName);
            var activatorType = GetActivatorType(LifeStyle.Singleton, serviceType);

            _container.Register(identity, activatorType);
            _singletonInstanceCache.Add(identity, instance);

            return this;
        }

        public ITypeRegistration Register<TService>(string serviceName, LifeStyle lifeStyle)
        {
            var serviceType = typeof(TService);
            var activatorType = GetActivatorType(lifeStyle, serviceType);
            var identity = new ServiceIdentity(serviceType, serviceName);

            _container.Register(identity, activatorType);

            return this;
        }

        public ITypeRegistration Register<TService, TInstance>(string serviceName, LifeStyle lifeStyle)
        {
            var serviceType = typeof(TService);
            var instanceType = typeof(TInstance);
            var identity = new ServiceIdentity(serviceType, serviceName);

            return Register(identity, instanceType, lifeStyle);
        }

        public ITypeRegistration Register(ServiceIdentity identity, LifeStyle lifeStyle)
        {
            return Register(identity, identity.ServiceType, lifeStyle);
        }

        public ITypeRegistration Register(ServiceIdentity identity, Type instanceType, LifeStyle lifeStyle)
        {
            var activatorType = GetActivatorType(lifeStyle, identity.ServiceType);

            _container.Register(identity, activatorType);

            if (instanceType != identity.ServiceType)
                _typeSubstitution.AddSubstitution(identity, instanceType);

            return this;
        }

        public ITypeRegistration RegisterActivator(ServiceIdentity identity, Type activatorType)
        {
            _container.Register(identity, activatorType);

            return this;
        }

        private Type GetActivatorType(LifeStyle lifeStyle, Type serviceType)
        {
            if (lifeStyle == LifeStyle.Default)
                lifeStyle = DefaultLifeStyle;

            switch (lifeStyle)
            {
                case LifeStyle.Singleton:
                    return typeof(SingletonActivator);

                case LifeStyle.Transient:
                    return typeof(TransientActivator);

                default:
                    throw new InvalidLifeStyleException(lifeStyle, serviceType);
            }
        }
    }
}