using System;

namespace Domo.Settings.ProviderBasedSettings.Serialization
{
    public interface ISettingsSerializer
    {
        Type SerializationType { get; }

        object Serialize<TValue>(TValue data);
        TValue Deserialize<TValue>(object data);
    }
}