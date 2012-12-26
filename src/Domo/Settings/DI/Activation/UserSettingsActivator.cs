using Domo.DI;
using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.Settings.DI.Activation
{
    [PreventAutomaticRegistration]
    public class UserSettingsActivator : SettingsActivator, IActivator
    {
        private readonly IUserSettings _userSettings;

        public UserSettingsActivator(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        public object ActivateService(IInjectionContext context, ServiceIdentity identity)
        {
            return
                GetGenericLoader(identity.ServiceType).
                LoadInstance(_userSettings, identity.ServiceName);
        }
    }
}