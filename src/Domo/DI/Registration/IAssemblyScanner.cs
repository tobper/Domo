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
        IAssemblyScanner ScanDirectory(string path);
        IAssemblyScanner ScanLoadedAssemblies();
        IAssemblyScanner UseConventionBasedProcessor();
        IAssemblyScanner UseScanProcessor(IScanProcessor scanProcessor);
        IAssemblyScanner UseScanProcessor(Func<IScanProcessor> scanProcessorDelegate);
        IAssemblyScanner UseScanProcessor<T>() where T : IScanProcessor, new();
    }
}