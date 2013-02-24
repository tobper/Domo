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

        public IServiceLocator ServiceLocator { get; private set; }

        private Container()
        {
            ServiceLocator = new ServiceLocator(this);

            var singletonCache = new DictionaryServiceCache();

            Configure(config => config.
                RegisterInstance(ServiceLocator).
                RegisterInstance<IContainer>(this).
                RegisterInstance<IServiceCache>(singletonCache));
        }

        public static IContainer Create(Action<IContainerConfiguration> config = null)
        {
            var container = new Container();

            if (config != null)
                container.Configure(config);

            return container;
        }

        public void Configure(Action<IContainerConfiguration> config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            var configuration = new ContainerConfiguration();

            config(configuration);

            configuration.ApplyRegistrations(this);
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
            var activator = GetActivator(identity);
            if (activator == null)
                return null;

            var context = CreateInjectionContext();
            var instance = activator.ActivateService(context);

            return instance;
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var activators = GetActivators(serviceType);
            if (activators == null)
                return null;

            var context = CreateInjectionContext();

            return from activator in activators
                   select activator.ActivateService(context);
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