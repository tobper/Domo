using System.Reflection;
using Domo.DI.Registration;

namespace Domo.Settings.DI.Registration
{
    public class SettingsScanConvention : IScanConvention
    {
        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            var settingsAttribute = type.GetCustomAttribute<SettingsAttribute>();
            if (settingsAttribute != null)
            {
                var settingsType = type.AsType();
                var settingsScope = settingsAttribute.Scope;
                var activatorConfiguration = new SettingsActivatorConfiguration(settingsScope, settingsType);

                // Todo: Settings are registered without name so it is not possible to load settings based on other argument names.

                container.Register(activatorConfiguration);
            }
        }
    }
}