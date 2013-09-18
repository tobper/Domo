using System.Threading.Tasks;
using InjectMe;
using InjectMe.Activation;

namespace Domo.Settings.DI.Activation
{
    public class UserSettingsActivator<TSettings> : IActivator
        where TSettings : class
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserSettings _userSettings;

        public ServiceIdentity Identity { get; private set; }

        public UserSettingsActivator(IServiceLocator serviceLocator, IUserSettings userSettings)
        {
            _serviceLocator = serviceLocator;
            _userSettings = userSettings;

            Identity = new ServiceIdentity(typeof(TSettings));
        }

        public object ActivateService(IActivationContext context)
        {
            return LoadValue().Result;
        }

        private async Task<TSettings> LoadValue()
        {
            var value = await _userSettings.Load<TSettings>();
            if (value == null)
            {
                var settingsHandler = _serviceLocator.TryResolve<ISettingsHandler<TSettings>>();
                if (settingsHandler != null)
                {
                    value = settingsHandler.GetDefaultSettings();
                }
            }

            return value;
        }
    }
}