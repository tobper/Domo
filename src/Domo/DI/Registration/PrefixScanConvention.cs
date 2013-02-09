using System.Reflection;

namespace Domo.DI.Registration
{
    public class PrefixScanConvention : IScanConvention
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
                var identity = serviceType.TryGetServiceIdentity(concreteType.Name);
                if (identity == null)
                    continue;

                container.
                    Register(identity).
                    AsTransient().
                    UsingConcreteType(concreteType.AsType());
            }
        }
    }
}