using System;
using System.Reflection;
using Domo.DI;
using Domo.DI.Registration;

namespace Domo.Messaging.DI
{
    public class MessageHandlerScanProcessor : IScanProcessor
    {
        private static readonly Type CommandHandlerTypeDefinition = typeof(ICommandHandler<>);
        private static readonly Type MessageHandlerTypeDefinition = typeof(IMessageHandler<>);

        public void ProcessType(ITypeRegistration typeRegistration, TypeInfo type)
        {
            foreach (var serviceType in type.ImplementedInterfaces)
            {
                if (!serviceType.IsConstructedGenericType)
                    continue;

                var typeDefinition = serviceType.GetGenericTypeDefinition();

                if (typeDefinition == CommandHandlerTypeDefinition ||
                    typeDefinition == MessageHandlerTypeDefinition)
                {
                    typeRegistration.Register(serviceType, type.AsType(), LifeStyle.Transient);
                }
            }
        }
    }
}