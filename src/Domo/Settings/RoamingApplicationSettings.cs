using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation.Collections;
using Domo.Extensions;

namespace Domo.Settings
{
    [SecuritySafeCritical]
    public class RoamingApplicationSettings : IApplicationSettings
    {
        private static IPropertySet Settings
        {
            get { return ApplicationData.Current.RoamingSettings.Values; }
        }

        [SecuritySafeCritical]
        public Task<T> Load<T>(string key = null)
        {
            var settingsKey = GetSettingsKey<T>(key);
            var value = (T)Settings[settingsKey];

            return value.AsTaskResult();
        }

        [SecuritySafeCritical]
        public Task<T[]> LoadAll<T>()
        {
            var settingsKey = GetSettingsKey<T>(null);

            return Task.Run(() =>
            {
                var values =
                    from setting in Settings
                    where setting.Key.StartsWith(settingsKey)
                    select (T)setting.Value;

                return values.ToArray();
            });
        }

        [SecuritySafeCritical]
        public Task Save<T>(T value, string key = null)
        {
            var settingsKey = GetSettingsKey<T>(key);

            Settings[settingsKey] = value;

            return Task.FromResult(0);
        }

        [SecuritySafeCritical]
        public Task<bool> Exists<T>(string key = null)
        {
            var settingsKey = GetSettingsKey<T>(key);
            var exists = Settings.ContainsKey(settingsKey);

            return exists.AsTaskResult();
        }

        private static string GetSettingsKey<T>(string key)
        {
            return string.Format("{0} {1}", typeof(T).FullName, key);
        }
    }
}