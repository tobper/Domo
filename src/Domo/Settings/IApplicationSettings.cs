using System.Threading.Tasks;

namespace Domo.Settings
{
    public interface IApplicationSettings
    {
        Task<T> Load<T>(string name = null);
        Task<T[]> LoadAll<T>();
        Task Save<T>(T value, string name = null);
        Task<bool> Exists<T>(string name = null);
    }
}