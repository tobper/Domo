using Domo.DI;
using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.Settings.DI.Activation
{
    [PreventAutomaticRegistration]
    public class ApplicationSettingsActivator : SettingsActivator, IActivator
    {
        private readonly IApplicationSettings _applicationSettings;

        public ApplicationSettingsActivator(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public object ActivateService(IInjectionContext context, ServiceIdentity identity)
        {
            return
                GetGenericLoader(identity.ServiceType).
                LoadInstance(_applicationSettings, identity.ServiceName);
        }
    }
}