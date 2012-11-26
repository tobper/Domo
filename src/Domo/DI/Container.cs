using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Domo.DI.Redirection;
using Domo.DI.Registration;
using Domo.Extensions;

namespace Domo.DI
{
    public class Container : IContainer
    {
        private readonly IDictionary<Type, IActivator> _activators;
        private readonly IDictionary<Type, IServiceFamily> _serviceFamilies;
        private readonly IFactoryManager _factoryManager;
        private readonly IServiceLocator _serviceLocator;

        private Container()
        {
            IInstanceCache singletonInstanceCache = new InstanceCache();
            ITypeRedirector typeRedirector = new TypeRedirector();

            _factoryManager = new FactoryManager(this);
            _serviceLocator = new ServiceLocator(this);
            _serviceFamilies = new Dictionary<Type, IServiceFamily>();
            _activators = new Dictionary<Type, IActivator>
            {
                { typeof(SingletonActivator), new SingletonActivator(_factoryManager, singletonInstanceCache, typeRedirector) },
                { typeof(TransientActivator), new TransientActivator(_factoryManager, typeRedirector) }
            };

            ITypeRegistration typeRegistration =
                new TypeRegistration(this, _factoryManager, singletonInstanceCache, typeRedirector);

            typeRegistration.
                RegisterSingleton<IContainer>(this).
                RegisterSingleton(_factoryManager).
                RegisterSingleton(_serviceLocator).
                RegisterSingleton(singletonInstanceCache, "Singleton").
                RegisterSingleton(typeRedirector).
                RegisterSingleton(typeRegistration).
                Register<IAssemblyScanner, AssemblyScanner>(LifeStyle.Singleton);
        }

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
            var typeRegistration = _serviceLocator.Resolve<ITypeRegistration>();

            registration(typeRegistration);
        }

        public void Scan(Action<IAssemblyScanner> scanner)
        {
            var assemblyScanner = _serviceLocator.Resolve<IAssemblyScanner>();

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
            var serviceActivator = GetActivator(serviceType, serviceName);

            return serviceActivator.ActivateInstance(activationContext, serviceType, serviceName);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);
            if (serviceFamily == null)
                throw new ServiceNotRegisteredException(serviceType);

            var activationContext = CreateActivationContext();
            var familyMembers = serviceFamily.GetMembers();

            foreach (var familyMember in familyMembers)
            {
                var activator = GetActivator(familyMember.ActivatorType);
                var instance = activator.ActivateInstance(activationContext, serviceType, familyMember.ServiceName);

                yield return instance;
            }
        }

        public IActivator GetActivator(Type serviceType, string serviceName)
        {
            var isLazy = serviceType.IsConstructedGenericType &&
                         serviceType.GetGenericTypeDefinition() == typeof(Lazy<>);

            if (isLazy)
                serviceType = serviceType.GenericTypeArguments[0];

            var activatorType = GetActivatorType(serviceType, serviceName);
            var activator = GetActivator(activatorType);

            if (isLazy)
                activator = new LazyActivator(activator);

            return activator;
        }

        private IActivator GetActivator(Type activatorType)
        {
            return _activators.TryGetValue(activatorType, CreateServiceActivator);
        }

        private Type GetActivatorType(Type serviceType, string serviceName)
        {
            var serviceFamily = _serviceFamilies.TryGetValue(serviceType);
            if (serviceFamily == null)
                throw new ServiceNotRegisteredException(serviceType);

            var activator = serviceFamily.GetActivator(serviceName);
            if (activator == null)
                throw new ServiceNotRegisteredException(serviceType, serviceName);

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