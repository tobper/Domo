using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Domo.DI.Redirection;
using Domo.DI.Registration;
using Domo.Extensions;

namespace Domo.DI
{
    using ActivationContext = Activation.ActivationContext;

    public class Container : IContainer
    {
        private readonly IDictionary<Type, IActivator> _activators;
        private readonly IDictionary<Type, IServiceFamily> _serviceFamilies;
        private readonly IFactoryManager _factoryManager;

        private Container()
        {
            IInstanceCache singletonInstanceCache = new InstanceCache();
            ITypeRedirector typeRedirector = new TypeRedirector();

            ServiceLocator = new ServiceLocator(this);

            _factoryManager = new FactoryManager(this);
            _serviceFamilies = new Dictionary<Type, IServiceFamily>();
            _activators = new Dictionary<Type, IActivator>
            {
                { typeof(SingletonFactoryActivator), new SingletonFactoryActivator(_factoryManager, singletonInstanceCache, typeRedirector) },
                { typeof(TransientFactoryActivator), new TransientFactoryActivator(_factoryManager, typeRedirector) }
            };

            ITypeRegistration typeRegistration =
                new TypeRegistration(this, _factoryManager, singletonInstanceCache, typeRedirector);

            typeRegistration.
                RegisterSingleton<IContainer>(this).
                RegisterSingleton(_factoryManager).
                RegisterSingleton(ServiceLocator).
                RegisterSingleton(singletonInstanceCache, "Singleton").
                RegisterSingleton(typeRedirector).
                RegisterSingleton(typeRegistration).
                Register<IAssemblyScanner, AssemblyScanner>(LifeStyle.Singleton);
        }

        public IServiceLocator ServiceLocator { get; private set; }

        public static IContainer Create(Action<ITypeRegistration> registration = null, Action<IAssemblyScanner> scanner = null)
        {
            IContainer container = new Container();

            if (registration != null)
                container.Register(registration);

            if (scanner != null)
                container.Scan(scanner);

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

        public void Register(Type serviceType, string serviceName, Type activatorType)
        {
            _serviceFamilies.
                TryGetValue(serviceType, () => new ServiceFamily(serviceType)).
                AddActivator(activatorType, serviceName);
        }

        public object Resolve(Type serviceType, string serviceName)
        {
            var activationContext = CreateActivationContext();
            var serviceActivator = TryGetActivator(serviceType, serviceName);

            if (serviceActivator == null)
                return null;

            return serviceActivator.ActivateInstance(activationContext, serviceType, serviceName);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);
            if (serviceFamily == null)
                yield break;

            var activationContext = CreateActivationContext();
            var familyMembers = serviceFamily.GetMembers();

            foreach (var familyMember in familyMembers)
            {
                var activator = TryGetActivator(familyMember.ActivatorType);
                var instance = activator.ActivateInstance(activationContext, serviceType, familyMember.ServiceName);

                yield return instance;
            }
        }

        public IActivator GetActivator(Type serviceType, string serviceName)
        {
            var activator = TryGetActivator(serviceType, serviceName);
            if (activator == null)
                throw new ServiceNotRegisteredException(serviceType, serviceName);

            return activator;
        }

        private IActivator TryGetActivator(Type serviceType, string serviceName)
        {
            var isLazy = serviceType.IsConstructedGenericType &&
                         serviceType.GetGenericTypeDefinition() == typeof(Lazy<>);

            if (isLazy)
                serviceType = serviceType.GenericTypeArguments[0];

            var activatorType = GetActivatorType(serviceType, serviceName);
            if (activatorType == null)
                return null;

            var activator = TryGetActivator(activatorType);
            if (activator == null)
                return null;

            if (isLazy)
                activator = new LazyActivator(activator);

            return activator;
        }

        private IActivator TryGetActivator(Type activatorType)
        {
            return _activators.TryGetValue(activatorType, CreateServiceActivator);
        }

        private Type GetActivatorType(Type serviceType, string serviceName)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);
            if (serviceFamily == null)
                return null;

            var activator = serviceFamily.GetActivator(serviceName);
            if (activator == null)
                return null;

            return activator;
        }

        private IActivator CreateServiceActivator(Type activatorType)
        {
            var activationContext = CreateActivationContext();
            var serviceFactory = _factoryManager.GetFactory(activatorType);

            return (IActivator)serviceFactory.CreateInstance(activationContext);
        }

        public IFactory GetFactory(Type serviceType)
        {
            return _factoryManager.GetFactory(serviceType);
        }

        //private TService CreateInstance<TService>()
        //{
        //    var serviceType = typeof(TService);
        //    var instance = (TService)CreateInstance(serviceType);

        //    return instance;
        //}

        //private object CreateInstance(Type type)
        //{
        //    var activationContext = CreateActivationContext();
        //    var factory = _factoryManager.GetFactory(type);
        //    var instance = factory.CreateInstance(activationContext);

        //    return instance;
        //}

        private ActivationContext CreateActivationContext()
        {
            return new ActivationContext(this);
        }
    }
}