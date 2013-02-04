using System;
using System.Reflection;
using Domo.DI.Registration;
using Domo.DI.Registration.Conventions;
using Domo.Settings.DI.Activation;

namespace Domo.Settings.DI.Registration
{
    public class SettingsScanConvention : IScanConvention
    {
        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            var settingsAttribute = type.GetCustomAttribute<SettingsAttribute>();
            if (settingsAttribute != null)
            {
                var activatorType = GetActivatorType(settingsAttribute.Scope);
                var settingsType = type.AsType();

                // Todo: Settings are registered without name so it is not possible to load settings based on other argument names.

                container.
                    Register(settingsType).
                    AsTransient().
                    ActivatedBy(activatorType);
            }
        }

        private static Type GetActivatorType(SettingsScope scope)
        {
            switch (scope)
            {
                case SettingsScope.Application:
                    return typeof(ApplicationSettingsActivator);

                case SettingsScope.User:
                    return typeof(UserSettingsActivator);

                default:
                    throw new InvalidOperationException("Invalid settings scope");
            }
        }
    }
}