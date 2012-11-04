using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domo.DI.Scanning
{
    using AssemblyFilter = Func<Assembly, bool>;
    using ServiceFilter = Func<ServiceInfo, bool>;

    public class AssemblyScanner : IAssemblyScanner
    {
        private readonly IServiceRegistration _registration;
        private readonly IList<AssemblyFilter> _assemblyFilters = new List<AssemblyFilter>();
        private readonly IList<ServiceFilter> _serviceFilters = new List<ServiceFilter>();
        private readonly IList<IServiceScanner> _serviceScanners = new List<IServiceScanner>();

        public AssemblyScanner(IServiceRegistration registration)
        {
            _registration = registration;
        }

        public IAssemblyScanner UseConventionBasedScanner()
        {
            return AddServiceScanner(new ConventionBasedServiceScanner());
        }

        public IAssemblyScanner AddAssemblyFilter(AssemblyFilter assemblyFilter)
        {
            _assemblyFilters.Add(assemblyFilter);
            return this;
        }

        public IAssemblyScanner AddServiceFilter(ServiceFilter serviceFilter)
        {
            _serviceFilters.Add(serviceFilter);
            return this;
        }

        public IAssemblyScanner AddServiceScanner(IServiceScanner serviceScanner)
        {
            _serviceScanners.Add(serviceScanner);
            return this;
        }

        public IAssemblyScanner ScanAssembly(Assembly assembly)
        {
            var services = GetServicesInAssembly(assembly);

            foreach (var serviceFilter in _serviceFilters)
            {
                services = services.Where(serviceFilter);
            }

            foreach (var serviceInfo in services)
            {
                _registration.RegisterService(serviceInfo);
            }

            return this;
        }

        public IAssemblyScanner ScanAssemblyContaining<T>()
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();
            var assembly = typeInfo.Assembly;

            return ScanAssembly(assembly);
        }

        public IAssemblyScanner ScanDirectory(string path)
        {
            var assemblies = GetAssembliesInPath(path);

            foreach (var assemblyFilter in _assemblyFilters)
            {
                assemblies = assemblies.Where(assemblyFilter);
            }

            foreach (var assembly in assemblies)
            {
                ScanAssembly(assembly);
            }

            return this;
        }

        private IEnumerable<Assembly> GetAssembliesInPath(string path)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ServiceInfo> GetServicesInAssembly(Assembly assembly)
        {
            return from type in assembly.DefinedTypes
                   from scanner in _serviceScanners
                   from service in scanner.GetServices(type)
                   select service;
        }
    }
}