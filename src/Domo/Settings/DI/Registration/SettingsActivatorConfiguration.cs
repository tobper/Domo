using System;
using Domo.DI;
using Domo.DI.Activation;
using Domo.DI.Construction;
using Domo.DI.Registration;
using Domo.Settings.DI.Activation;

namespace Domo.Settings.DI.Registration
{
    public class SettingsActivatorConfiguration : IActivatorConfiguration
    {
        protected SettingsScope SettingsScope { get; private set; }
        protected Type SettingsType { get; private set; }

        public SettingsActivatorConfiguration(SettingsScope settingsScope, Type settingsType)
        {
            SettingsScope = settingsScope;
            SettingsType = settingsType;
        }

        public IActivator GetService(IContainer container)
        {
            var serviceType = GetServiceType(SettingsType, SettingsScope);
            var service = (IActivator)serviceType.ConstructInstance(container);

            return service;
        }

        private static Type GetServiceType(Type settingsType, SettingsScope scope)
        {
            switch (scope)
            {
                case SettingsScope.Application:
                    return typeof(ApplicationSettingsActivator<>).MakeGenericType(settingsType);

                case SettingsScope.User:
                    return typeof(UserSettingsActivator<>).MakeGenericType(settingsType);

                default:
                    throw new InvalidOperationException("Invalid settings scope");
            }
        }
    }
}