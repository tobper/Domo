using System;
using System.Collections.Generic;
using System.Reflection;
using Domo.DI;
using Domo.DI.Scanning;

namespace Domo.Messaging.DI
{
    public class MessageHandlerServiceScanner : IServiceScanner
    {
        private static readonly Type MessageHandlerTypeDefinition = typeof(IMessageHandler<>);

        public IEnumerable<ServiceInfo> GetServices(TypeInfo type)
        {
            foreach (var serviceType in type.ImplementedInterfaces)
            {
                if (!serviceType.IsConstructedGenericType)
                    continue;

                var typeDefinition = serviceType.GetGenericTypeDefinition();
                if (typeDefinition == MessageHandlerTypeDefinition)
                    yield return new ServiceInfo(serviceType, type.AsType(), LifeStyle.Transient);
            }
        }
    }
}