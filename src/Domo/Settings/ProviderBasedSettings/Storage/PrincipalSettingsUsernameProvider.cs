using System.Threading;
using System.Threading.Tasks;
using Domo.Extensions;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public class PrincipalSettingsUsernameProvider : ISettingsUsernameProvider
    {
        public Task<string> GetUserName()
        {
            var name = Thread.CurrentPrincipal.Identity.Name;

            return name.AsTaskResult();
        }
    }
}