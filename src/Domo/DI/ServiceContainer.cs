using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Scanning;
using Domo.Extensions;

namespace Domo.DI
{
    using ServiceInfoDictionary = Dictionary<Type, List<ServiceInfo>>;
    using ServiceActivatorDictionary = Dictionary<Type, IServiceActivator>;

    public class ServiceContainer : IServiceContainer
    {
        private readonly ServiceInfoDictionary _services = new ServiceInfoDictionary();
        private readonly ServiceActivatorDictionary _activators = new ServiceActivatorDictionary();
        private readonly IServiceCache _singletonCache = new ServiceCache();
        private LifeStyle _defaultLifeStyle = LifeStyle.Singleton;

        public LifeStyle DefaultLifeStyle
        {
            get { return _defaultLifeStyle; }
            set
            {
                if (value == LifeStyle.Default)
                    throw new ArgumentException("Default life style must be set to a concrete life style.");

                _defaultLifeStyle = value;
            }
        }

        private ServiceContainer()
        {
        }

        public static ServiceContainer Create(Action<IServiceRegistration> registration = null)
        {
            var container = new ServiceContainer();
            container.Register(r =>
            {
                r.RegisterType<IServiceLocator, ServiceLocator>(LifeStyle.Singleton);
                r.RegisterSingleton<IServiceContainer>(container);

                if (registration != null)
                    registration(r);
            });
            return container;
        }

        public void Register(Action<IServiceRegistration> registration)
        {
            var serviceRegistration = new ServiceRegistration(this, _services, _singletonCache);

            registration(serviceRegistration);
        }

        public IEnumerable<object> Resolve(Type serviceType)
        {
            var activationContext = new ActivationContext(_singletonCache, null);
            return ResolveServices(serviceType, activationContext);
        }

        private IEnumerable<object> ResolveServices(Type serviceType, ActivationContext activationContext)
        {
            var activators = GetActivators(serviceType);
            if (activators == null)
                throw new ServiceNotRegisteredException(serviceType);

            foreach (var activator in activators)
            {
                yield return activator.ActivateInstance(activationContext);
            }
        }

        private IEnumerable<IServiceActivator> GetActivators(Type serviceType)
        {
            var services = _services.TryGetValue(serviceType);
            if (services == null)
                throw new ServiceNotRegisteredException(serviceType);

            foreach (var serviceInfo in services)
            {
                yield return GetActivator(serviceInfo);
            }
        }

        private IServiceActivator GetActivator(ServiceInfo serviceInfo)
        {
            return _activators.TryGetValue(serviceInfo.ActivationType, () => CreateActivator(serviceInfo));
        }

        private IServiceActivator GetDefaultActivator(Type serviceType)
        {
            // Todo: Pick registered default instead of first
            // Todo: Add support for enumerable arguments
            return
                GetActivators(serviceType).
                First();
        }

        private IServiceActivator CreateActivator(ServiceInfo serviceInfo)
        {
            var lifeStyle = GetLifeStyle(serviceInfo);
            var instanceFactory = CreateInstanceFactory(serviceInfo);

            switch (lifeStyle)
            {
                case LifeStyle.Scoped:
                    return new ScopedServiceActivator(serviceInfo.ActivationType, instanceFactory);

                case LifeStyle.Singleton:
                    return new SingletonServiceActivator(serviceInfo.ActivationType, instanceFactory);

                case LifeStyle.Transient:
                    return new TransientServiceActivator(instanceFactory);

                default:
                    throw new InvalidLifeStyleException(serviceInfo);
            }
        }

        private LifeStyle GetLifeStyle(ServiceInfo serviceInfo)
        {
            return (serviceInfo.LifeStyle == LifeStyle.Default)
                ? DefaultLifeStyle
                : serviceInfo.LifeStyle;
        }

        private IInstanceFactory CreateInstanceFactory(ServiceInfo serviceInfo)
        {
            var delegates = from constructor in serviceInfo.ActivationType.GetTypeInfo().DeclaredConstructors
                            let parameters = constructor.GetParameters()
                            orderby parameters.Length descending
                            select CreateInstanceFactory(constructor, parameters);

            var factoryDelegate = delegates.FirstOrDefault();
            if (factoryDelegate == null)
                throw new NoValidConstructorFoundException(serviceInfo.ActivationType);

            return factoryDelegate;
        }

        private IInstanceFactory CreateInstanceFactory(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            var activators = parameters.Convert(parameter =>
            {
                var serviceType = parameter.ParameterType;
                return GetDefaultActivator(serviceType);
            });

            return new InstanceFactory(constructor, activators);
        }
    }
}