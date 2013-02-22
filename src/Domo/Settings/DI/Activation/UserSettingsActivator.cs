using Domo.DI;
using Domo.DI.Activation;

namespace Domo.Settings.DI.Activation
{
    public class UserSettingsActivator<TSettings> : IActivator
    {
        private readonly IUserSettings _userSettings;

        public ServiceIdentity Identity { get; private set; }

        public UserSettingsActivator(IUserSettings userSettings)
        {
            _userSettings = userSettings;

            Identity = new ServiceIdentity(typeof(TSettings));
        }

        public object GetService(IInjectionContext context)
        {
            var value = _userSettings.Load<TSettings>();

            return value.Result;
        }
    }
}