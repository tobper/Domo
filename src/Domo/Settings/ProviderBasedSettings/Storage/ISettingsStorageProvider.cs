using System;
using System.Threading.Tasks;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public interface ISettingsStorageProvider
    {
        bool SupportsSerializationType(Type storageType);

        Task<object> Load(Type valueType, string user, string name, Type storageType);
        Task<Setting[]> LoadAll(Type storageType);
        Task Save(Type valueType, string user, string name, object value);
        Task<bool> Exists(Type valueType, string user, string name);
    }
}