using System;
using Domo.DI.Activation;

namespace Domo.Settings.DI.Activation
{
    public class ApplicationSettingsActivator : SettingsActivator, IActivator
    {
        private readonly IApplicationSettings _applicationSettings;

        public ApplicationSettingsActivator(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public object ActivateInstance(ActivationContext activationContext, Type type, string name)
        {
            return
                GetGenericActivator(type).
                ActivateInstance(_applicationSettings, name);
        }
    }
}