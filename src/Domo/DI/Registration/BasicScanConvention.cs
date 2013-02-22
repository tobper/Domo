using System.Reflection;
using System.Linq;
using Domo.DI.Caching;

namespace Domo.DI.Registration
{
    public class BasicScanConvention : IScanConvention
    {
        private static readonly IServiceScope DefaultServiceScope = new TransientScope();

        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition || type.IsNested)
                return;

            if (type.Namespace != null &&
                type.Namespace.StartsWith("System"))
                return;

            ProcessConcreteType(container, type);
        }

        private static void ProcessConcreteType(IContainerConfiguration container, TypeInfo concreteTypeInfo)
        {
            var serviceTypeName = "I" + concreteTypeInfo.Name;
            var serviceType = concreteTypeInfo.ImplementedInterfaces.FirstOrDefault(i => i.Name == serviceTypeName);

            if (serviceType != null)
            {
                var serviceTypeInfo = serviceType.GetTypeInfo();
                var scope = GetScope(concreteTypeInfo, serviceTypeInfo);

                container.
                    Register(serviceType).
                    InScope(scope).
                    UsingConcreteType(concreteTypeInfo.AsType());
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