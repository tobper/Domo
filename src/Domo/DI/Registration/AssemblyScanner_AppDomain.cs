using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domo.DI.Registration
{
    public partial class AssemblyScanner
    {
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

        private static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}