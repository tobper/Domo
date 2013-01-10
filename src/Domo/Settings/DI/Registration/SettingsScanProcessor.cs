using System;
using System.Reflection;
using Domo.DI;
using Domo.DI.Registration;
using Domo.Settings.DI.Activation;

namespace Domo.Settings.DI.Registration
{
    public class SettingsScanProcessor : IScanProcessor
    {
        public void ProcessType(ITypeRegistration typeRegistration, TypeInfo type)
        {
            var settingsAttribute = type.GetCustomAttribute<SettingsAttribute>();
            if (settingsAttribute != null)
            {
                var activatorType = GetActivatorType(settingsAttribute.Scope);
                var identity = new ServiceIdentity(type.AsType());
                // Todo: Settings are only registered without name so it is not possible to load settings based on other argument names.

                typeRegistration.RegisterActivator(identity, activatorType);
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