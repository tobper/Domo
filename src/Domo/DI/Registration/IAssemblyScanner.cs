using System;
using System.Reflection;

namespace Domo.DI.Registration
{
    public interface IAssemblyScanner
    {
        IAssemblyScanner AddAssemblyFilter(Func<Assembly, bool> assemblyFilter);
        IAssemblyScanner AddTypeFilter(Func<TypeInfo, bool> typeFilter);
        IAssemblyScanner AddScanProcessor(IScanProcessor scanProcessor);
        IAssemblyScanner ScanAssembly(Assembly assembly);
        IAssemblyScanner ScanAssemblyContaining<T>();
        IAssemblyScanner ScanDirectory(string path);
        IAssemblyScanner UseConventionBasedScanner();
    }
}