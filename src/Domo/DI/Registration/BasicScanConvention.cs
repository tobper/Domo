using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Domo.DI.Caching;

namespace Domo.DI.Registration
{
    public class BasicScanConvention : IScanConvention
    {
        private static readonly IServiceScope DefaultServiceScope = new TransientScope();

        public bool UsePrefixResolution { get; private set; }

        public BasicScanConvention(bool usePrefixResolution)
        {
            UsePrefixResolution = usePrefixResolution;
        }

        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition || type.IsNested)
                return;

            if (type.Namespace != null &&
                type.Namespace.StartsWith("System"))
                return;

            ProcessConcreteType(container, type);
        }

        private void ProcessConcreteType(IContainerConfiguration container, TypeInfo concreteTypeInfo)
        {
            var identities = GetIdentities(concreteTypeInfo);

            foreach (var identity in identities)
            {
                var serviceTypeInfo = identity.ServiceType.GetTypeInfo();
                var scope = GetScope(concreteTypeInfo, serviceTypeInfo);

                container.
                    Register(identity).
                    InScope(scope).
                    UsingConcreteType(concreteTypeInfo.AsType());
            }
        }

        private IEnumerable<ServiceIdentity> GetIdentities(TypeInfo concreteType)
        {
            if (UsePrefixResolution)
            {
                foreach (var serviceType in concreteType.ImplementedInterfaces)
                {
                    var identity = serviceType.TryGetServiceIdentity(concreteType.Name);
                    if (identity != null)
                        yield return identity;
                }
            }
            else
            {
                var serviceTypeName = "I" + concreteType.Name;
                var serviceType = concreteType.ImplementedInterfaces.FirstOrDefault(i => i.Name == serviceTypeName);

                if (serviceType != null)
                {
                    yield return new ServiceIdentity(serviceType);
                }
            }
        }

        private static IServiceScope GetScope(TypeInfo concreteType, TypeInfo serviceType)
        {
            var concreteTypeAttribute = concreteType.GetCustomAttribute<ServiceScopeAttribute>();
            if (concreteTypeAttribute != null)
                return concreteTypeAttribute.Scope;

            var serviceTypeAttribute = serviceType.GetCustomAttribute<ServiceScopeAttribute>();
            if (serviceTypeAttribute != null)
                return serviceTypeAttribute.Scope;

            return DefaultServiceScope;
        }
    }
}