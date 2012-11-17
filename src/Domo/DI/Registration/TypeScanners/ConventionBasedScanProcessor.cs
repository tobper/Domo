using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Domo.DI.Registration.TypeScanners
{
    public class ConventionBasedScanProcessor : IScanProcessor
    {
        public void ProcessType(ITypeRegistration typeRegistration, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition)
                return;

            // Extract interfaces with prefixes
            // I.e. SingletonServiceCache should register a IServiceCache service named Singleton
            foreach (var serviceType in type.ImplementedInterfaces)
            {
                var serviceName = GetServiceName(serviceType, type.Name);
                if (serviceName == null)
                    continue;

                if (serviceName == string.Empty)
                    serviceName = null;

                typeRegistration.Register(serviceType, type.AsType(), serviceName: serviceName);
            }
        }

        private string GetServiceName(Type serviceType, string referenceName)
        {
            var serviceTypeName = Regex.Replace(serviceType.Name, "^I", string.Empty);
            var match = Regex.Match(referenceName, "i?(.*?)" + serviceTypeName);

            if (match.Success)
                return match.Groups[1].Value;

            return null;
        }
    }
}