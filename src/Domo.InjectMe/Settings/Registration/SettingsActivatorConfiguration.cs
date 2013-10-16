using System;
using Domo.Settings.Activation;
using InjectMe;
using InjectMe.Activation;
using InjectMe.Construction;
using InjectMe.Registration;

namespace Domo.Settings.Registration
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