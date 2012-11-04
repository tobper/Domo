using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domo.DI.Scanning
{
    public class ConventionBasedServiceScanner : IServiceScanner
    {
        public IEnumerable<ServiceInfo> GetServices(TypeInfo type)
        {
            var typeName = type.Name;
            var serviceType = type.ImplementedInterfaces.FirstOrDefault(s => s.Name == "I" + typeName);
            if (serviceType != null)
                yield return new ServiceInfo(serviceType, type.AsType());
        }
    }
}