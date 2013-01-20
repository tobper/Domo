using System;
using System.Collections.Generic;
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

            _serviceFamilies.
                TryGetValue(identity.ServiceType, () => new ServiceFamily(identity.ServiceType)).
                AddActivator(identity, activatorType);
        }

        public object Resolve(ServiceIdentity identity)
        {
            var activator = TryGetActivator(identity);
            if (activator == null)
                return null;

            var context = CreateInjectionContext();

            return activator.ActivateService(context, identity);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);
            if (serviceFamily == null)
                yield break;

            var context = CreateInjectionContext();
            var familyMembers = serviceFamily.GetMembers();

            foreach (var familyMember in familyMembers)
            {
                var activator = _activators[familyMember.ActivatorType];
                var instance = activator.ActivateService(context, familyMember.Identity);

                yield return instance;
            }
        }

        // Todo: GetActivator will throw if service has not been registered but Resolve(...) will not
        public IActivator GetActivator(ServiceIdentity identity)
        {
            var activator = TryGetActivator(identity);
            if (activator == null)
                throw new ServiceNotRegisteredException(identity);

            return activator;
        }

        private IActivator TryGetActivator(ServiceIdentity identity)
        {
            var isLazy = identity.ServiceType.IsConstructedGenericType &&
                         identity.ServiceType.GetGenericTypeDefinition() == typeof(Lazy<>);

            if (isLazy)
            {
                identity = new ServiceIdentity(
                    identity.ServiceType.GenericTypeArguments[0],
                    identity.ServiceName);
            }

            var activatorType = GetActivatorType(identity);
            if (activatorType == null)
                return null;

            var activator = _activators[activatorType];

            if (isLazy)
                activator = new LazyActivator(activator);

            return activator;
        }

        private Type GetActivatorType(ServiceIdentity identity)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(identity.ServiceType);
            if (serviceFamily == null)
                return null;

            var activator = serviceFamily.GetActivator(identity);
            if (activator == null)
                return null;

            return activator;
        }

        private IInjectionContext CreateInjectionContext()
        {
            return new InjectionContext(this);
        }
    }
}