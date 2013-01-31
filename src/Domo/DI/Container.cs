using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Domo.DI.Registration;
using Domo.Extensions;

namespace Domo.DI
{
    public class Container : IContainer
    {
        private readonly IDictionary<Type, IServiceFamily> _serviceFamilies;
        private readonly IActivatorContainer _activators;

        private Container()
        {
            IInstanceCache singletonInstanceCache = new InstanceCache();
            ITypeSubstitution typeSubstitution = new TypeSubstitution();
            IFactoryContainer factoryContainer = new FactoryContainer(this);

            ServiceLocator = new ServiceLocator(this);

            _serviceFamilies = new Dictionary<Type, IServiceFamily>();
            _activators = new ActivatorContainer(
                factoryContainer,
                new SingletonActivator(factoryContainer, typeSubstitution, singletonInstanceCache),
                new TransientActivator(factoryContainer, typeSubstitution));

            ITypeRegistration typeRegistration =
                new TypeRegistration(this, factoryContainer, singletonInstanceCache, typeSubstitution);

            typeRegistration.
                RegisterSingleton<IContainer>(this).
                RegisterSingleton(factoryContainer).
                RegisterSingleton(ServiceLocator).
                RegisterSingleton(singletonInstanceCache, "Singleton").
                RegisterSingleton(typeSubstitution).
                RegisterSingleton(typeRegistration).
                Register<IAssemblyScanner, AssemblyScanner>();
        }

        public IServiceLocator ServiceLocator { get; private set; }

        public static IContainer Create(ContainerConfigurationDelegate configuration)
        {
            IContainer container = new Container();
            var typeRegistration = container.ServiceLocator.Resolve<ITypeRegistration>();
            var assemblyScanner = container.ServiceLocator.Resolve<IAssemblyScanner>();

            configuration(container, typeRegistration, assemblyScanner);

            return container;
        }

        public void Register(Action<ITypeRegistration> registration)
        {
            var typeRegistration = ServiceLocator.Resolve<ITypeRegistration>();

            registration(typeRegistration);
        }

        public void Scan(Action<IAssemblyScanner> scanner)
        {
            var assemblyScanner = ServiceLocator.Resolve<IAssemblyScanner>();

            scanner(assemblyScanner);
        }

        public void Register(ServiceIdentity identity, Type activatorType)
        {
            if (!activatorType.Implements<IActivator>())
                throw new InvalidActivatorTypeException(activatorType);

            var activator = _activators.GetActivator(activatorType);

            _serviceFamilies.
                TryGetValue(identity.ServiceType, () => new ServiceFamily(identity.ServiceType)).
                AddActivator(identity, activator);
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

        // Todo: GetActivator will throw if service has not been registered but Resolve(...) will not
        public ActivationDelegate GetActivationDelegate(ServiceIdentity identity)
        {
            var activator = TryGetActivationDelegate(identity);
            if (activator == null)
                throw new ServiceNotRegisteredException(identity);

            return activator;
        }

        private ActivationDelegate TryGetActivationDelegate(ServiceIdentity identity)
        {
            var isLazy = IsLazyIdentity(identity);
            if (isLazy)
            {
                identity = new ServiceIdentity(
                    identity.ServiceType.GenericTypeArguments[0],
                    identity.ServiceName);
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