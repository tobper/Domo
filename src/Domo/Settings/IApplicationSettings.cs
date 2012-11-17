namespace Domo.Settings
{
    public interface IApplicationSettings
    {
        T Load<T>(string key = null);
        void Save<T>(T value, string key = null);
        bool Exists<T>(string key = null);
    }
}