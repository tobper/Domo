using System;
using System.Linq;
using System.Reflection;
using InjectMe.Caching;
using InjectMe.Registration;

namespace Domo.Messaging.Registration
{
    public class MessagingScanConvention : IScanConvention
    {
        private static readonly Type[] HandlerTypeDefinitions =
        {
            typeof(ICommandHandler<>),
            typeof(IMessageHandler<>)
        };

        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            var serviceTypes = from serviceType in type.ImplementedInterfaces
                               where serviceType.IsConstructedGenericType
                               select serviceType;

            foreach (var serviceType in serviceTypes)
            {
                ProcessType(container, type, serviceType);
            }
        }

        private static void ProcessType(IContainerConfiguration container, TypeInfo concreteType, Type serviceType)
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