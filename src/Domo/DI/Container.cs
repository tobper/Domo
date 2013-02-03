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
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);
            if (serviceFamily == null)
                return Enumerable.Empty<object>();

            var context = CreateInjectionContext();
            var services = serviceFamily.GetAllServices();

            return from service in services
                   select service.GetInstance(context);
        }

        public IService GetService(ServiceIdentity identity)
        {
            var isLazy = IsLazyIdentity(identity);
            if (isLazy)
            {
                var realServiceType = identity.ServiceType.GenericTypeArguments[0];

                identity = new ServiceIdentity(realServiceType, identity.ServiceName);
            }

            var serviceFamily = _serviceFamilies.TryGetValue(identity.ServiceType);
            if (serviceFamily == null)
                return null;

            var service = serviceFamily.GetService(identity);

            if (isLazy)
                service = new LazyService(service);

            return service;
        }

        private static bool IsLazyIdentity(ServiceIdentity identity)
        {
            return identity.ServiceType.IsConstructedGenericType &&
                   identity.ServiceType.GetGenericTypeDefinition() == typeof(Lazy<>);
        }

        private IInjectionContext CreateInjectionContext()
        {
            return new InjectionContext(this);
        }
    }
}