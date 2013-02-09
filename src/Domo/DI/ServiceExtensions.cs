using System;
using System.Text.RegularExpressions;

namespace Domo.DI
{
    public static class ServiceExtensions
    {
        // IFoo, "SpecialFoo"      -> IFoo, "Special"
        // IFoo, "Foo"             -> IFoo, null
        // IFoo<int>, "SpecialFoo" -> IFoo<int>, "Special"
        // IFoo<int>, "Foo"        -> IFoo<int>, null
        // Foo, "SpecialFoo"       -> Foo, "Special"
        // Foo, "Foo"              -> Foo, null
        // IFoo, "Something"       -> null
        public static ServiceIdentity TryGetServiceIdentity(this Type serviceType, string referenceName)
        {
            // Strip away potential leading I and trailing generic construct.
            var serviceTypeName = Regex.Replace(serviceType.Name, @"^I|`\d+$", string.Empty);

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