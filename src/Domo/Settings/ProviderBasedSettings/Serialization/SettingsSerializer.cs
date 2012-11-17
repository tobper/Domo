using System;

namespace Domo.Settings.ProviderBasedSettings.Serialization
{
    public abstract class SettingsSerializer<TStorage> : ISettingsSerializer
    {
        protected abstract TStorage Serialize<TValue>(TValue value);
        protected abstract TValue Deserialize<TValue>(TStorage data);

        Type ISettingsSerializer.SerializationType
        {
            get { return typeof(TStorage); }
        }

        object ISettingsSerializer.Serialize<TValue>(TValue value)
        {
            return Serialize(value);
        }

        TValue ISettingsSerializer.Deserialize<TValue>(object data)
        {
            return Deserialize<TValue>((TStorage)data);
        }
    }
}