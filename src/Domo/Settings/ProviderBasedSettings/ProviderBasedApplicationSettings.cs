using System;
using Domo.DI.Registration;
using Domo.Settings.ProviderBasedSettings.Serialization;
using Domo.Settings.ProviderBasedSettings.Storage;

namespace Domo.Settings.ProviderBasedSettings
{
    [PreventAutomaticRegistration]
    public class ProviderBasedApplicationSettings : IApplicationSettings
    {
        private readonly ISettingsStorageProvider _storageProvider;
        private readonly ISettingsSerializer _serializer;

        public ProviderBasedApplicationSettings(ISettingsStorageProvider storageProvider, ISettingsSerializer serializer)
        {
            if (storageProvider == null)
                throw new ArgumentNullException("storageProvider");

            if (serializer == null)
                throw new ArgumentNullException("serializer");

            if (!storageProvider.SupportsSerializationType(serializer.SerializationType))
                throw new InvalidSerializationTypeException();

            _storageProvider = storageProvider;
            _serializer = serializer;
        }

        public T Load<T>(string name = null)
        {
            var data = _storageProvider.Load(typeof(T), null, name, _serializer.SerializationType);
            var value = _serializer.Deserialize<T>(data);

            return value;
        }

        public void Save<T>(T value, string name = null)
        {
            var data = _serializer.Serialize(value);

            _storageProvider.Save(typeof(T), null, name, data);
        }

        public bool Exists<T>(string name = null)
        {
            return _storageProvider.Exists(typeof(T), null, name);
        }
    }
}