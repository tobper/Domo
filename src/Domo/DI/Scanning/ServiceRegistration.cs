using System;
using System.Collections.Generic;
using Domo.DI.Caching;
using Domo.Extensions;

namespace Domo.DI.Scanning
{
    using ServiceInfoDictionary = Dictionary<Type, List<ServiceInfo>>;

    public class ServiceRegistration : IServiceRegistration
    {
        private readonly ServiceInfoDictionary _services;
        private readonly IServiceCache _singletonCache;

        public IServiceContainer Container { get; set; }

        public ServiceRegistration(IServiceContainer serviceContainer, ServiceInfoDictionary services, IServiceCache singletonCache)
        {
            Container = serviceContainer;

            _services = services;
            _singletonCache = singletonCache;
        }

        public IServiceRegistration RegisterService(ServiceInfo serviceInfo)
        {
            _services.Add(serviceInfo.ServiceType, serviceInfo);
            return this;
        }

        public IServiceRegistration RegisterSingleton<TService>(TService service) where TService : class
        {
            if (service == null)
                throw new ArgumentNullException("service");

            var serviceInfo = new ServiceInfo(typeof(TService), service.GetType(), LifeStyle.Singleton);
            _services.Add(serviceInfo.ServiceType, serviceInfo);
            _singletonCache.Add(serviceInfo.ActivationType, service);

            return this;
        }

        public IServiceRegistration RegisterType<TService, TActivation>(LifeStyle lifeStyle = LifeStyle.Default)
        {
            return RegisterType(typeof(TService), typeof(TActivation), lifeStyle);
        }

        public IServiceRegistration RegisterType(Type serviceType, Type activationType, LifeStyle lifeStyle = LifeStyle.Default)
        {
            var serviceInfo = new ServiceInfo(serviceType, activationType, lifeStyle);
            return RegisterService(serviceInfo);
        }

        public IServiceRegistration Scan(Action<IAssemblyScanner> scanner)
        {
            var assemblyScanner = new AssemblyScanner(this);
            scanner(assemblyScanner);
            return this;
        }
    }
}