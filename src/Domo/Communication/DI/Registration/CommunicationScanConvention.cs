using System;
using System.Linq;
using System.Reflection;
using InjectMe.Caching;
using InjectMe.Registration;

namespace Domo.Communication.DI.Registration
{
    public class CommunicationScanConvention : IScanConvention
    {
        private static readonly Type[] HandlerTypeDefinitions = new[]
        {
            typeof(ICommandHandler<>),
            typeof(IMessageHandler<>)
        };

        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            foreach (var serviceType in type.ImplementedInterfaces)
            {
                ProcessType(container, type, serviceType);
            }
        }

        private static void ProcessType(IContainerConfiguration container, TypeInfo concreteType, Type serviceType)
        {
            if (serviceType.IsConstructedGenericType)
            {
                var typeDefinition = serviceType.GetGenericTypeDefinition();

                if (HandlerTypeDefinitions.Any(h => h == typeDefinition))
                {
                    var serviceScope =
                        concreteType.GetServiceScope() ??
                        ServiceScope.Default;

                    container.
                        Register(serviceType).
                        InScope(serviceScope).
                        UsingConcreteType(concreteType.AsType());
                }
            }
        }
    }
}