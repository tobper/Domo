using System;
using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.Settings.DI.Activation
{
    using ActivationContext = Domo.DI.Activation.ActivationContext;

    [PreventAutomaticRegistration]
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