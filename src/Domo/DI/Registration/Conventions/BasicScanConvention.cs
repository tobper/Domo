using System.Reflection;

namespace Domo.DI.Registration.Conventions
{
    public class BasicScanConvention : IScanConvention
    {
        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition || type.IsNested)
                return;

            if (type.Namespace != null &&
                type.Namespace.StartsWith("System"))
                return;

            ProcessConcreteType(container, type);
        }

        private static void ProcessConcreteType(IContainerConfiguration container, TypeInfo concreteType)
        {
            // Extract interfaces with prefixes
            // I.e. SingletonServiceCache should register a IServiceCache service named Singleton
            foreach (var serviceType in concreteType.ImplementedInterfaces)
            {
                var identity = serviceType.GetServiceIdentity(concreteType.Name);
                if (identity == null)
                    continue;

                container.
                    Register(identity).
                    Using(concreteType);
            }
        }
    }
}