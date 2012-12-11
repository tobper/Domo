namespace Domo.Settings
{
    public interface IUserSettings
    {
        T Load<T>(string name = null);
        void Save<T>(T value, string name = null);
        bool Exists<T>(string name = null);
    }
}