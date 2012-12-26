using System;

namespace Domo.DI
{
    public static class ServiceExtensions
    {
        // IFoo, "SpecialFoo" -> IFoo, "Special"
        // IFoo, "Foo"        -> IFoo, null
        // Foo, "SpecialFoo"  -> Foo, "Special"
        // Foo, "Foo"         -> Foo, null
        // IFoo, "Something"  -> null
        public static ServiceIdentity GetServiceIdentity(this Type serviceType, string referenceName)
        {
            var serviceTypeName = (serviceType.Name.StartsWith("I"))
                ? serviceType.Name.Substring(1)
                : serviceType.Name;

            if (referenceName.EndsWith(serviceTypeName, StringComparison.OrdinalIgnoreCase))
            {
                var serviceNameLength = referenceName.Length - serviceTypeName.Length;
                var serviceName = (serviceNameLength > 0)
                    ? referenceName.Substring(0, serviceNameLength)
                    : null;

                return new ServiceIdentity(serviceType, serviceName);
            }

            return null;
        }
    }
}