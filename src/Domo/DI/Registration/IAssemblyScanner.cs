using System;
using System.Reflection;

namespace Domo.DI.Registration
{
    public interface IAssemblyScanner
    {
        IAssemblyScanner AddAssemblyFilter(Func<Assembly, bool> assemblyFilter);
        IAssemblyScanner AddTypeFilter(Func<TypeInfo, bool> typeFilter);
        IAssemblyScanner ScanAssembly(Assembly assembly);
        IAssemblyScanner ScanAssemblyContaining<T>();
        IAssemblyScanner UseConvention(IScanConvention convention);
        IAssemblyScanner UseConvention(Func<IScanConvention> conventionDelegate);
        IAssemblyScanner UseConvention<T>() where T : IScanConvention, new();

#if !NETFX_CORE
        IAssemblyScanner ScanLoadedAssemblies();
#endif
    }
}