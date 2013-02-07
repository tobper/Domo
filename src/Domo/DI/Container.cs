using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Construction;
using Domo.DI.Registration;
using Domo.Extensions;

namespace Domo.DI
{
    public class Container : IContainer
    {
        private readonly IDictionary<Type, IServiceFamily> _serviceFamilies = new Dictionary<Type, IServiceFamily>();

        private Container()
        {
            ServiceLocator = new ServiceLocator(this);

            var singletonCache = new InstanceCache();
            var typeSubstitution = new TypeSubstitution();
            var constructionFactories = new ConstructionFactoryContainer(this);
            var factories = new FactoryContainer(this, constructionFactories);
            var activators = new ActivatorContainer(
                factories,
                new SingletonActivator(factories, typeSubstitution, singletonCache),
                new TransientActivator(factories, typeSubstitution));

            new ContainerConfiguration(this).
                Register(ServiceLocator).
                Register<IActivatorContainer>(activators).
                Register<IConstructionFactoryContainer>(constructionFactories).
                Register<IContainer>(this).
                Register<IFactoryContainer>(factories).
                Register<IInstanceCache>(singletonCache, "Singleton").
                Register<ITypeSubstitution>(typeSubstitution).
                Register<IAssemblyScanner, AssemblyScanner>().
                Register<IContainerConfiguration, ContainerConfiguration>().
                CompleteRegistration();
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
                configuration.CompleteRegistration();
            }

            if (scan != null)
            {
                var scanner = ServiceLocator.Resolve<IAssemblyScanner>();

                scan(scanner);
            }
        }

        public void Register(IService service)
        {
            var serviceType = service.Identity.ServiceType;

            _serviceFamilies.
                TryGetValue(serviceType, () => new ServiceFamily(serviceType)).
                Add(service);
        }

        public object Resolve(ServiceIdentity identity)
        {
            var service = GetService(identity);
            if (service == null)
                return null;

            var context = CreateInjectionContext();
            var instance = service.GetInstance(context);

            return instance;
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var services = GetServices(serviceType);
            if (services == null)
                return null;

            var context = CreateInjectionContext();

            return from service in services
                   select service.GetInstance(context);
        }

        public IService GetService(ServiceIdentity identity)
        {
            return
                TryGetRegisteredService(identity) ??
                TryCreateArrayService(identity) ??
                TryCreateGenericService(identity);
        }

        private IService TryGetRegisteredService(ServiceIdentity identity)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(identity.ServiceType);
            if (serviceFamily != null)
                return serviceFamily.GetService(identity);

            return null;
        }

        private IService TryCreateArrayService(ServiceIdentity identity)
        {
            if (identity.ServiceType.IsArray)
            {
                var itemServiceType = identity.ServiceType.GetElementType();
                var itemServices = GetServices(itemServiceType);

                return new ArrayService(identity, itemServiceType, itemServices);
            }

            return null;
        }

        private IService TryCreateGenericService(ServiceIdentity identity)
        {
            if (identity.ServiceType.IsConstructedGenericType)
            {
                var genericTypeDefinition = identity.ServiceType.GetGenericTypeDefinition();
                var realServiceType = identity.ServiceType.GenericTypeArguments[0];

                if (genericTypeDefinition == typeof(Func<>))
                {
                    var realIdentity = new ServiceIdentity(realServiceType, identity.ServiceName);
                    var realService = GetService(realIdentity);

                    if (realService != null)
                        return new FuncService(identity, realService);
                }
                else if (genericTypeDefinition == typeof(Lazy<>))
                {
                    var realIdentity = new ServiceIdentity(realServiceType, identity.ServiceName);
                    var realService = GetService(realIdentity);

                    if (realService != null)
                        return new LazyService(identity, realService);
                }
                else if (genericTypeDefinition == typeof(IEnumerable<>) ||
                         genericTypeDefinition == typeof(ICollection<>) ||
                         genericTypeDefinition == typeof(IList<>))
                {
                    var itemServices = GetServices(realServiceType);

                    return new ArrayService(identity, realServiceType, itemServices);
                }
            }

            return null;
        }

        public IService[] GetServices(Type serviceType)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);

            return (serviceFamily != null)
                ? serviceFamily.GetAllServices()
                : new IService[0];
        }

        private IInjectionContext CreateInjectionContext()
        {
            return new InjectionContext(this);
        }
    }
}