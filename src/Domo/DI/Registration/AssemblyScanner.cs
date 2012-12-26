using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Domo.DI.Registration.TypeScanners;

namespace Domo.DI.Registration
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private static readonly Type PreventAutomaticRegistrationAttributeType = typeof(PreventAutomaticRegistrationAttribute);

        private readonly ITypeRegistration _typeRegistration;
        private readonly IList<Func<Assembly, bool>> _assemblyFilters = new List<Func<Assembly, bool>>();
        private readonly IList<Func<TypeInfo, bool>> _typeFilters = new List<Func<TypeInfo, bool>>();
        private readonly IList<IScanProcessor> _typeScanners = new List<IScanProcessor>();

        public AssemblyScanner(ITypeRegistration typeRegistration)
        {
            _typeRegistration = typeRegistration;

            AddTypeFilter(type =>
                !IsAnonymousType(type) &&
                !IsManuallyRegisteredType(type) &&
                !HasPreventionAttribute(type)
            );
        }

        public IAssemblyScanner UseConventionBasedProcessor()
        {
            return UseScanProcessor(new ConventionBasedScanProcessor());
        }

        public IAssemblyScanner UseScanProcessor(IScanProcessor scanProcessor)
        {
            _typeScanners.Add(scanProcessor);
            return this;
        }

        public IAssemblyScanner UseScanProcessor(Func<IScanProcessor> scanProcessorDelegate)
        {
            var scanProcessor = scanProcessorDelegate();
            return UseScanProcessor(scanProcessor);
        }

        public IAssemblyScanner UseScanProcessor<T>() where T : IScanProcessor, new()
        {
            var scanProcessor = new T();
            return UseScanProcessor(scanProcessor);
        }

        public IAssemblyScanner AddAssemblyFilter(Func<Assembly, bool> assemblyFilter)
        {
            _assemblyFilters.Add(assemblyFilter);
            return this;
        }

        public IAssemblyScanner AddTypeFilter(Func<TypeInfo, bool> typeFilter)
        {
            _typeFilters.Add(typeFilter);
            return this;
        }

        public IAssemblyScanner ScanAssembly(Assembly assembly)
        {
            Trace.WriteLine("Domo: Scanning assembly {0}.", assembly.FullName);

            var types = assembly.DefinedTypes;

            foreach (var typeFilter in _typeFilters)
            {
                types = types.Where(typeFilter);
            }

            foreach (var type in types)
            {
                foreach (var typeScanner in _typeScanners)
                {
                    typeScanner.ProcessType(_typeRegistration, type);
                }
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

        public IAssemblyScanner ScanLoadedAssemblies()
        {
            var assemblies = GetLoadedAssemblies();

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsAnonymousType(TypeInfo type)
        {
            return type.Name.StartsWith("<>");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsManuallyRegisteredType(TypeInfo type)
        {
            return
                type.Namespace != null &&
                type.Namespace.StartsWith("Domo.DI");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasPreventionAttribute(TypeInfo type)
        {
            return type.IsDefined(PreventAutomaticRegistrationAttributeType);
        }

        private static IEnumerable<Assembly> GetAssembliesInPath(string path)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Assembly> GetLoadedAssemblies()
        {
#if NETFX_CORE
            throw new NotSupportedException("Scanning loaded assemblies is not supported in a Windows Store app.");
#else
            return AppDomain.CurrentDomain.GetAssemblies();
#endif
        }
    }
}