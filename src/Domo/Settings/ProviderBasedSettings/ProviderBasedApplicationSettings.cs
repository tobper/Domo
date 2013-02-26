using System;
using System.Threading.Tasks;
using Domo.DI.Registration;
using Domo.Settings.ProviderBasedSettings.Serialization;
using Domo.Settings.ProviderBasedSettings.Storage;
using Domo.Extensions;

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

        public async Task<T> Load<T>(string name = null)
        {
            var data = await _storageProvider.Load(typeof(T), null, name, _serializer.SerializationType);
            var value = TryDeserializeValue<T>(data);

            return value;
        }

        public async Task<T[]> LoadAll<T>()
        {
            var settings = await _storageProvider.LoadAll(typeof(T), _serializer.SerializationType);
            var values = settings.Convert(s => _serializer.Deserialize<T>(s.Value));

            return values;
        }

        public async Task Save<T>(T value, string name = null)
        {
            var data = TrySerializeValue(value);

            await _storageProvider.Save(typeof(T), null, name, data);
        }

        public async Task<bool> Exists<T>(string name = null)
        {
            return await _storageProvider.Exists(typeof(T), null, name);
        }

        private object TrySerializeValue<T>(T value)
        {
            if (Equals(value, default(T)))
                return null;
            
            return _serializer.Serialize(value);
        }

        private T TryDeserializeValue<T>(object data)
        {
            if (data == null)
                return default(T);

            return _serializer.Deserialize<T>(data);
        }
    }
}