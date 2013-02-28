using System.Threading.Tasks;
using Domo.DI;
using Domo.DI.Activation;

namespace Domo.Settings.DI.Activation
{
    public class ApplicationSettingsActivator<TSettings> : IActivator
        where TSettings : class
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IApplicationSettings _applicationSettings;

        public ServiceIdentity Identity { get; private set; }

        public ApplicationSettingsActivator(IServiceLocator serviceLocator, IApplicationSettings applicationSettings)
        {
            _serviceLocator = serviceLocator;
            _applicationSettings = applicationSettings;

            Identity = new ServiceIdentity(typeof(TSettings));
        }

        public object ActivateService(IInjectionContext context)
        {
            return LoadValue().Result;
        }

        private async Task<TSettings> LoadValue()
        {
            var value = await _applicationSettings.Load<TSettings>();
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