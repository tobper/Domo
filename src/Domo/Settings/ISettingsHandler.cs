namespace Domo.Settings
{
    public interface ISettingsHandler<out TSettings>
    {
        TSettings GetDefaultSettings();
    }
}