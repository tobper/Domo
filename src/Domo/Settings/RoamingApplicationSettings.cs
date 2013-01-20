using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation.Collections;
using Domo.Extensions;

namespace Domo.Settings
{
    public class RoamingApplicationSettings : IApplicationSettings
    {
        private static IPropertySet Settings
        {
            get { return ApplicationData.Current.RoamingSettings.Values; }
        }

        public Task<T> Load<T>(string key = null)
        {
            var settingsKey = GetSettingsKey<T>(key);
            var value = (T)Settings[settingsKey];

            return value.AsTaskResult();
        }

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

        public Task Save<T>(T value, string key = null)
        {
            var settingsKey = GetSettingsKey<T>(key);

            Settings[settingsKey] = value;

            return null;
        }

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