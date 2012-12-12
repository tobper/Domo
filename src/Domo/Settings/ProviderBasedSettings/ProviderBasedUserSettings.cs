using System;
using Domo.DI.Registration;
using Domo.Settings.ProviderBasedSettings.Serialization;
using Domo.Settings.ProviderBasedSettings.Storage;

namespace Domo.Settings.ProviderBasedSettings
{
    [PreventAutomaticRegistration]
    public class ProviderBasedUserSettings : IUserSettings
    {
        private readonly ISettingsUsernameProvider _usernameProvider;
        private readonly ISettingsStorageProvider _storageProvider;
        private readonly ISettingsSerializer _serializer;

        public ProviderBasedUserSettings(ISettingsUsernameProvider userNameProvider, ISettingsStorageProvider storageProvider, ISettingsSerializer serializer)
        {
            if (userNameProvider == null)
                throw new ArgumentNullException("userNameProvider");

            if (storageProvider == null)
                throw new ArgumentNullException("storageProvider");

            if (serializer == null)
                throw new ArgumentNullException("serializer");

            if (!storageProvider.SupportsSerializationType(serializer.SerializationType))
                throw new InvalidSerializationTypeException();

            _usernameProvider = userNameProvider;
            _storageProvider = storageProvider;
            _serializer = serializer;
        }

        public T Load<T>(string name = null)
        {
            var user = _usernameProvider.GetUserName();
            var data = _storageProvider.Load(typeof(T), user, name, _serializer.SerializationType);
            var value = DeserializeValue<T>(data);

            return value;
        }

        public void Save<T>(T value, string name = null)
        {
            var user = _usernameProvider.GetUserName();
            var data = SerializeValue(value);

            _storageProvider.Save(typeof(T), user, name, data);
        }

        public bool Exists<T>(string name = null)
        {
            var user = _usernameProvider.GetUserName();
            return _storageProvider.Exists(typeof(T), user, name);
        }

        private object SerializeValue<T>(T value)
        {
            if (Equals(value, default(T)))
                return null;
            
            return _serializer.Serialize(value);
        }

        private T DeserializeValue<T>(object data)
        {
            if (data == null)
                return default(T);

            return _serializer.Deserialize<T>(data);
        }
    }
}