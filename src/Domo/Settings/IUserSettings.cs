using System.Threading.Tasks;

namespace Domo.Settings
{
    public interface IUserSettings
    {
        Task<T> Load<T>(string name = null);
        Task Save<T>(T value, string name = null);
        Task<bool> Exists<T>(string name = null);
    }
}