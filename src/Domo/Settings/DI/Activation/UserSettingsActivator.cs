using System;
using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.Settings.DI.Activation
{
    using ActivationContext = Domo.DI.Activation.ActivationContext;

    [PreventAutomaticRegistration]
    public class UserSettingsActivator : SettingsActivator, IActivator
    {
        private readonly IUserSettings _userSettings;

        public UserSettingsActivator(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        public object ActivateInstance(ActivationContext activationContext, Type type, string name)
        {
            return
                GetGenericActivator(type).
                ActivateInstance(_userSettings, name);
        }
    }
}