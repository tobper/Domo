using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Registration;
using Domo.Extensions;

namespace Domo.DI
{
    public class Container : IContainer
    {
        private readonly IDictionary<Type, IActivatorGroup> _activatorGroups = new Dictionary<Type, IActivatorGroup>();

        private Container()
        {
            ServiceLocator = new ServiceLocator(this);

            var singletonCache = new DictionaryInstanceCache();

            new ContainerConfiguration().
                RegisterInstance(ServiceLocator).
                RegisterInstance<IContainer>(this).
                RegisterInstance<IInstanceCache>(singletonCache).
                RegisterTransient<IAssemblyScanner, AssemblyScanner>().
                RegisterTransient<IContainerConfiguration, ContainerConfiguration>().
                ApplyRegistrations(this);
        }

        public IServiceLocator ServiceLocator { get; private set; }

        public static IContainer Create(
            Action<IContainerConfiguration> configure = null,
            Action<IAssemblyScanner> scan = null)
        {
            IContainer container = new Container();
            container.Configure(configure, scan);
            return container;
        }

        public void Configure(
            Action<IContainerConfiguration> configure = null,
            Action<IAssemblyScanner> scan = null)
        {
            if (configure != null)
            {
                var configuration = ServiceLocator.Resolve<IContainerConfiguration>();

                configure(configuration);
                configuration.ApplyRegistrations(this);
            }

            if (scan != null)
            {
                var scanner = ServiceLocator.Resolve<IAssemblyScanner>();

                scan(scanner);
            }
        }

        public void Register(IActivator activator)
        {
            var serviceType = activator.Identity.ServiceType;

            _activatorGroups.
                TryGetValue(serviceType, () => new ActivatorGroup(serviceType)).
                Add(activator);
        }

        public object Resolve(ServiceIdentity identity)
        {
            var service = GetActivator(identity);
            if (service == null)
                return null;

            var context = CreateInjectionContext();
            var instance = service.GetInstance(context);

            return instance;
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var services = GetActivators(serviceType);
            if (services == null)
                return null;

            var context = CreateInjectionContext();

            return from service in services
                   select service.GetInstance(context);
        }

        public IActivator GetActivator(ServiceIdentity identity)
        {
            return
                TryGetRegisteredActivator(identity) ??
                TryCreateArrayActivator(identity) ??
                TryCreateGenericActivator(identity);
        }

        private IActivator TryGetRegisteredActivator(ServiceIdentity identity)
        {
            var activatorGroup = _activatorGroups.TryGetValue(identity.ServiceType);
            if (activatorGroup != null)
                return activatorGroup.GetActivator(identity);

            return null;
        }

        private IActivator TryCreateArrayActivator(ServiceIdentity identity)
        {
            if (identity.ServiceType.IsArray)
            {
                var itemServiceType = identity.ServiceType.GetElementType();
                var itemActivators = GetActivators(itemServiceType);

                return new ArrayActivator(identity, itemServiceType, itemActivators);
            }

            return null;
        }

        private IActivator TryCreateGenericActivator(ServiceIdentity identity)
        {
            if (identity.ServiceType.IsConstructedGenericType)
            {
                var genericTypeDefinition = identity.ServiceType.GetGenericTypeDefinition();
                var realServiceType = identity.ServiceType.GenericTypeArguments[0];

                if (genericTypeDefinition == typeof(Func<>))
                {
                    var realIdentity = new ServiceIdentity(realServiceType, identity.ServiceName);
                    var realActivator = GetActivator(realIdentity);

                    if (realActivator != null)
                        return new FuncActivator(identity, realActivator);
                }
                else if (genericTypeDefinition == typeof(Lazy<>))
                {
                    var realIdentity = new ServiceIdentity(realServiceType, identity.ServiceName);
                    var realActivator = GetActivator(realIdentity);

                    if (realActivator != null)
                        return new LazyActivator(identity, realActivator);
                }
                else if (genericTypeDefinition == typeof(IEnumerable<>) ||
                         genericTypeDefinition == typeof(ICollection<>) ||
                         genericTypeDefinition == typeof(IList<>))
                {
                    var itemActivators = GetActivators(realServiceType);

                    return new ArrayActivator(identity, realServiceType, itemActivators);
                }
            }

            return null;
        }

        public IActivator[] GetActivators(Type serviceType)
        {
            var activatorGroup = _activatorGroups.TryGetValue(serviceType);

            return (activatorGroup != null)
                ? activatorGroup.GetAllActivators()
                : new IActivator[0];
        }

        private IInjectionContext CreateInjectionContext()
        {
            return new InjectionContext(this);
        }
    }
}