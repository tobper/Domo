using Windows.Storage;

namespace Domo.Settings.DI.RoamingSettings
{
    public class RoamingApplicationSettings : IApplicationSettings
    {
        public T Load<T>(string key = null)
        {
            var combinedKey = GetKey<T>(key);
            var value = (T)ApplicationData.Current.RoamingSettings.Values[combinedKey];

            return value;
        }

        public void Save<T>(T value, string key = null)
        {
            var combinedKey = GetKey<T>(key);

            ApplicationData.Current.RoamingSettings.Values[combinedKey] = value;
        }

        public bool Exists<T>(string key = null)
        {
            var combinedKey = GetKey<T>(key);

            return ApplicationData.Current.RoamingSettings.Values.ContainsKey(combinedKey);
        }

        private static string GetKey<T>(string key)
        {
            return string.Format("{0} - {1}", typeof(T).Name, key);
        }
    }
}