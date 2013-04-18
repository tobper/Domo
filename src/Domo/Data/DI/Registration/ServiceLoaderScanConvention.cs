using System;
using System.Reflection;
using Domo.DI.Registration;

namespace Domo.Data.DI.Registration
{
    public class ServiceLoaderScanConvention : IScanConvention
    {
        private static readonly Type DataLoaderTypeDefinition = typeof(IServiceLoader<>);

        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            foreach (var serviceType in type.ImplementedInterfaces)
            {
                if (serviceType.IsConstructedGenericType)
                {
                    var typeDefinition = serviceType.GetGenericTypeDefinition();
                    if (typeDefinition == DataLoaderTypeDefinition)
                    {
                        container.
                            Register(serviceType).
                            AsTransient().
                            UsingConcreteType(type.AsType());

                        var dataType = serviceType.GenericTypeArguments[0];
                        var dataTypeConfiguration = new ServiceLoaderActivatorConfiguration(dataType);

                        container.Register(dataTypeConfiguration);
                    }
                }
            }
        }
    }
}