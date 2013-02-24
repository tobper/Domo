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

        public IActivator GetActivator(IContainer container)
        {
            var activatorType = GetActivatorType(SettingsType, SettingsScope);
            var activator = (IActivator)activatorType.ConstructInstance(container);

            return activator;
        }

        private static Type GetActivatorType(Type settingsType, SettingsScope scope)
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