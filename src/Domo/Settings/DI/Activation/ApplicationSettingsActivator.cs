using Domo.DI;
using Domo.DI.Activation;

namespace Domo.Settings.DI.Activation
{
    public class ApplicationSettingsActivator<TSettings> : IActivator
    {
        private readonly IApplicationSettings _applicationSettings;

        public ServiceIdentity Identity { get; private set; }

        public ApplicationSettingsActivator(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;

            Identity = new ServiceIdentity(typeof(TSettings));
        }

        public object GetInstance(IInjectionContext context)
        {
            var value = _applicationSettings.Load<TSettings>();

            return value.Result;
        }
    }
}