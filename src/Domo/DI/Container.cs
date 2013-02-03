using System;
using System.Collections.Generic;
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
            Action<IContainerConfiguration> registration = null,
            Action<IAssemblyScanner> scanner = null)
        {
            IContainer container = new Container();
            container.Configure(registration, scanner);
            return container;
        }

        public void Configure(
            Action<IContainerConfiguration> registration = null,
            Action<IAssemblyScanner> scanner = null)
        {
            if (registration != null)
            {
                var containerRegistration = ServiceLocator.Resolve<IContainerConfiguration>();

                registration(containerRegistration);
                containerRegistration.CompleteRegistration();
            }

            if (scanner != null)
            {
                var assemblyScanner = ServiceLocator.Resolve<IAssemblyScanner>();

                scanner(assemblyScanner);
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
            var activationDelegate = GetActivationDelegate(identity);
            if (activationDelegate == null)
                return null;

            var context = CreateInjectionContext();
            var instance = activationDelegate(context);

            return instance;
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);
            if (serviceFamily == null)
                yield break;

            var context = CreateInjectionContext();
            var activationDelegates = serviceFamily.GetAllActivationDelegates();

            foreach (var activationDelegate in activationDelegates)
            {
                var instance = activationDelegate(context);

                yield return instance;
            }
        }

        public ActivationDelegate GetActivationDelegate(ServiceIdentity identity)
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

            var activationDelegate = serviceFamily.GetActivationDelegate(identity);

            if (isLazy)
                activationDelegate = CreateLazyActivationDelegate(identity.ServiceType, activationDelegate);

            return activationDelegate;
        }

        private static bool IsLazyIdentity(ServiceIdentity identity)
        {
            return identity.ServiceType.IsConstructedGenericType &&
                   identity.ServiceType.GetGenericTypeDefinition() == typeof(Lazy<>);
        }

        private static ActivationDelegate CreateLazyActivationDelegate(Type serviceType, ActivationDelegate activationDelegate)
        {
            return context => Activator.CreateInstance(
                serviceType,
                CreateLazyFactoryDelegate(activationDelegate, context));
        }

        private static Func<object> CreateLazyFactoryDelegate(ActivationDelegate activationDelegate, IInjectionContext context)
        {
            return () => activationDelegate(context);
        }

        private IInjectionContext CreateInjectionContext()
        {
            return new InjectionContext(this);
        }
    }
}