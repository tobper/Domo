using System.Reflection;

namespace Domo.DI.Registration.TypeScanners
{
    public class ConventionBasedScanProcessor : IScanProcessor
    {
        public void ProcessType(ITypeRegistration typeRegistration, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition || type.IsNested)
                return;

            if (type.Namespace != null &&
                type.Namespace.StartsWith("System"))
                return;

            // Extract interfaces with prefixes
            // I.e. SingletonServiceCache should register a IServiceCache service named Singleton
            foreach (var serviceType in type.ImplementedInterfaces)
            {
                var identity = serviceType.GetServiceIdentity(type.Name);
                if (identity == null)
                    continue;

                typeRegistration.Register(identity, type.AsType());
            }
        }
    }
}