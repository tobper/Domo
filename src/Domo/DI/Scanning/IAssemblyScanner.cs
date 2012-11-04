using System;
using System.Reflection;

namespace Domo.DI.Scanning
{
    public interface IAssemblyScanner
    {
        IAssemblyScanner AddAssemblyFilter(Func<Assembly, bool> assemblyFilter);
        IAssemblyScanner AddServiceFilter(Func<ServiceInfo, bool> serviceFilter);
        IAssemblyScanner AddServiceScanner(IServiceScanner serviceScanner);
        IAssemblyScanner ScanAssembly(Assembly assembly);
        IAssemblyScanner ScanAssemblyContaining<T>();
        IAssemblyScanner ScanDirectory(string path);
        IAssemblyScanner UseConventionBasedScanner();
    }
}