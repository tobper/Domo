using System;
using System.Reflection;
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
                typeRegistration.Register(type.AsType(), activatorType);
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