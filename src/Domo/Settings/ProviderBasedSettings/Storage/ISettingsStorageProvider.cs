using System;
using System.Collections.Generic;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public interface ISettingsStorageProvider
    {
        bool SupportsSerializationType(Type storageType);

        object Load(Type valueType, string user, string name, Type storageType);
        void Save(Type valueType, string user, string name, object value);
        bool Exists(Type valueType, string user, string name);

        IEnumerable<Setting> LoadAll(Type storageType);
    }
}