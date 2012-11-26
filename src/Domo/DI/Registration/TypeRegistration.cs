using System;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Domo.DI.Redirection;

namespace Domo.DI.Registration
{
    public class TypeRegistration : ITypeRegistration
    {
        private readonly IContainer _container;
        private readonly IFactoryManager _factoryManager;
        private readonly IInstanceCache _singletonInstanceCache;
        private readonly ITypeRedirector _typeRedirector;

        public LifeStyle DefaultLifeStyle { get; private set; }

        public TypeRegistration(
            IContainer container,
            IFactoryManager factoryManager,
            IInstanceCache singletonInstanceCache,
            ITypeRedirector typeRedirector)
        {
            _container = container;
            _factoryManager = factoryManager;
            _singletonInstanceCache = singletonInstanceCache;
            _typeRedirector = typeRedirector;

            DefaultLifeStyle = LifeStyle.Singleton;
        }

        public ITypeRegistration SetDefaultLifeStyle(LifeStyle lifeStyle)
        {
            if (lifeStyle == LifeStyle.Default)
                throw new InvalidLifeStyleException(lifeStyle);

            DefaultLifeStyle = lifeStyle;

            return this;
        }

        public ITypeRegistration RegisterSingleton<TService>(TService instance, string serviceName) where TService : class
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            var serviceType = typeof(TService);
            var activatorType = GetActivatorType(LifeStyle.Singleton, serviceType);

            _container.Register(serviceType, serviceName, activatorType);
            _singletonInstanceCache.Add(serviceType, serviceName, instance);

            return this;
        }

        public ITypeRegistration RegisterDelegate<TService>(Func<ActivationContext, object> factoryDelegate, LifeStyle lifeStyle)
        {
            if (factoryDelegate == null)
                throw new ArgumentNullException("factoryDelegate");

            var serviceType = typeof(TService);
            var activatorType = GetActivatorType(lifeStyle, serviceType);
            var factory = new DelegateFactory(factoryDelegate);

            _factoryManager.AddFactory(serviceType, factory);
            _container.Register(serviceType, null, activatorType);

            return this;
        }

        public ITypeRegistration Register<TService>(LifeStyle lifeStyle, string serviceName)
        {
            var serviceType = typeof(TService);
            var activatorType = GetActivatorType(lifeStyle, serviceType);

            _container.Register(serviceType, serviceName, activatorType);

            return this;
        }

        public ITypeRegistration Register<TService, TInstance>(LifeStyle lifeStyle, string serviceName)
        {
            var serviceType = typeof(TService);
            var instanceType = typeof(TInstance);

            return Register(serviceType, instanceType, lifeStyle, serviceName);
        }

        public ITypeRegistration Register(Type serviceType, Type instanceType, LifeStyle lifeStyle, string serviceName)
        {
            var activatorType = GetActivatorType(lifeStyle, serviceType);

            _container.Register(serviceType, serviceName, activatorType);

            if (instanceType != serviceType)
                _typeRedirector.AddRedirection(serviceType, serviceName, instanceType);

            return this;
        }

        private Type GetActivatorType(LifeStyle lifeStyle, Type serviceType)
        {
            if (lifeStyle == LifeStyle.Default)
                lifeStyle = DefaultLifeStyle;

            switch (lifeStyle)
            {
                case LifeStyle.Singleton:
                    return typeof(SingletonFactoryActivator);

                case LifeStyle.Transient:
                    return typeof(TransientFactoryActivator);

                default:
                    throw new InvalidLifeStyleException(lifeStyle, serviceType);
            }
        }
    }
}