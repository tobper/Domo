using System.Threading.Tasks;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public interface ISettingsUsernameProvider
    {
        Task<string> GetUserName();
    }
}