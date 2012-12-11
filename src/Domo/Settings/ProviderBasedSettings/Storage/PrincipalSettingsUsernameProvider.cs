using System.Threading;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public class PrincipalSettingsUsernameProvider : ISettingsUsernameProvider
    {
        public string GetUserName()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }
    }
}