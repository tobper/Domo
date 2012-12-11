using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Registration;
using Domo.Extensions;
using System.Reflection;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    [PreventAutomaticRegistration]
    public class MemoryStorageProvider : ISettingsStorageProvider
    {
        private readonly Dictionary<string, Setting> _settings = new Dictionary<string, Setting>();
        private readonly Dictionary<Type, Version> _versions = new Dictionary<Type, Version>();

        protected void Add(Setting setting)
        {
            var completeKey = GetKey(setting.Type, setting.User, setting.Name);
            _settings[completeKey] = setting;
        }

        public bool SupportsSerializationType(Type storageType)
        {
            // Since instances are stored in memory, all types are supported.
            return true;
        }

        public object Load(Type valueType, string user, string name, Type storageType)
        {
            var completeKey = GetKey(valueType, user, name);
            var setting = _settings.TryGetValue(completeKey);

            return setting != null ? setting.Value : null;
        }

        public void Save(Type valueType, string user, string name, object value)
        {
            var setting = new Setting
            {
                Name = name,
                Type = valueType,
                User = user,
                Version = GetTypeVersion(valueType),
                Value = value,
            };

            Add(setting);
        }

        public bool Exists(Type valueType, string user, string name)
        {
            var completeKey = GetKey(valueType, user, name);
            return _settings.ContainsKey(completeKey);
        }

        public IEnumerable<Setting> LoadAll(Type storageType)
        {
            return _settings.Values.ToArray();
        }

        private Version GetTypeVersion(Type valueType)
        {
            return _versions.TryGetValue(valueType, () =>
            {
                var typeInfo = valueType.GetTypeInfo();
                var assemblyName = typeInfo.Assembly.GetName();

                return assemblyName.Version;
            });
        }

        private static string GetKey(Type valueType, string user, string name)
        {
            return string.Format("{0}, {1} - {2}", user, valueType.FullName, name);
        }
    }
}