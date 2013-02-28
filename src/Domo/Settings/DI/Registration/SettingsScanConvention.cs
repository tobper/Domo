using System;
using System.Reflection;
using Domo.DI.Caching;
using Domo.DI.Registration;

namespace Domo.Settings.DI.Registration
{
    public class SettingsScanConvention : IScanConvention
    {
        private static readonly Type SettingsHandlerTypeDefinition = typeof(ISettingsHandler<>);

        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            var settingsAttribute = type.GetCustomAttribute<SettingsAttribute>();
            if (settingsAttribute != null)
            {
                var settingsType = type.AsType();
                var settingsScope = settingsAttribute.Scope;
                var activatorConfiguration = new SettingsActivatorConfiguration(settingsScope, settingsType);

                container.Register(activatorConfiguration);
            }

            foreach (var serviceType in type.ImplementedInterfaces)
            {
                ProcessServiceType(container, type, serviceType);
            }
        }

        private static void ProcessServiceType(IContainerConfiguration container, TypeInfo concreteType, Type serviceType)
        {
            if (serviceType.IsConstructedGenericType)
            {
                var typeDefinition = serviceType.GetGenericTypeDefinition();
                if (typeDefinition == SettingsHandlerTypeDefinition)
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