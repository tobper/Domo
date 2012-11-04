using System.Collections.Generic;
using System.Reflection;

namespace Domo.DI.Scanning
{
    public interface IServiceScanner
    {
        IEnumerable<ServiceInfo> GetServices(TypeInfo type);
    }
}