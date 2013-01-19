﻿using System.Threading.Tasks;
using Windows.Storage;
using Domo.Extensions;

namespace Domo.Settings.DI.RoamingSettings
{
    public class RoamingApplicationSettings : IApplicationSettings
    {
        public Task<T> Load<T>(string key = null)
        {
            var combinedKey = GetKey<T>(key);
            var value = (T)ApplicationData.Current.RoamingSettings.Values[combinedKey];

            return value.AsTaskResult();
        }

        public Task Save<T>(T value, string key = null)
        {
            var combinedKey = GetKey<T>(key);

            ApplicationData.Current.RoamingSettings.Values[combinedKey] = value;

            return null;
        }

        public Task<bool> Exists<T>(string key = null)
        {
            var combinedKey = GetKey<T>(key);
            var exists = ApplicationData.Current.RoamingSettings.Values.ContainsKey(combinedKey);

            return exists.AsTaskResult();
        }

        private static string GetKey<T>(string key)
        {
            return string.Format("{0} - {1}", typeof(T).Name, key);
        }
    }
}