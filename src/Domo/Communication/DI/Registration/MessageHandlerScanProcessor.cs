using System;
using System.Linq;
using System.Reflection;
using Domo.DI;
using Domo.DI.Registration;

namespace Domo.Communication.DI.Registration
{
    public class MessageHandlerScanProcessor : IScanProcessor
    {
        private static readonly Type[] HandlerTypeDefinitions = new[]
        {
            typeof(ICommandHandler<>),
            typeof(IMessageHandler<>),
            typeof(IQueryHandler<,>)
        };

        public void ProcessType(ITypeRegistration typeRegistration, TypeInfo type)
        {
            foreach (var serviceType in type.ImplementedInterfaces)
            {
                if (serviceType.IsConstructedGenericType)
                {
                    var typeDefinition = serviceType.GetGenericTypeDefinition();

                    if (HandlerTypeDefinitions.Any(h => h == typeDefinition))
                    {
                        var identity = new ServiceIdentity(serviceType);

                        typeRegistration.Register(identity, type.AsType(), LifeStyle.Transient);
                    }
                }
            }
        }
    }
}