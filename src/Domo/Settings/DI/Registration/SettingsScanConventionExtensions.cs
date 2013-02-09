using Domo.DI.Registration;
using Domo.Settings.ProviderBasedSettings;
using Domo.Settings.ProviderBasedSettings.Serialization;
using Domo.Settings.ProviderBasedSettings.Storage;

namespace Domo.Settings.DI.Registration
{
    public static class SettingsScanConventionExtensions
    {
        /// <summary>
        /// The Settings conventions will automatically register all types marked with <see cref="SettingsAttribute"/>.
        /// </summary>
        public static IAssemblyScanner UseSettingsConventions(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseConvention<SettingsScanConvention>();
        }

        public static IContainerConfiguration RegisterProviderBasedSettings<TSettingsSerializer, TSettingsStorageProvider>(this IContainerConfiguration configuration)
            where TSettingsSerializer : ISettingsSerializer
            where TSettingsStorageProvider : ISettingsStorageProvider
        {
            return configuration.
                RegisterSingleton<ISettingsSerializer, TSettingsSerializer>().
                RegisterSingleton<ISettingsStorageProvider, TSettingsStorageProvider>().
                RegisterSingleton<IApplicationSettings, ProviderBasedApplicationSettings>();
        }

        public static IContainerConfiguration RegisterProviderBasedSettings<TSettingsSerializer, TSettingsStorageProvider, TSettingsUsernameProvider>(this IContainerConfiguration configuration)
            where TSettingsSerializer : ISettingsSerializer
            where TSettingsStorageProvider : ISettingsStorageProvider
            where TSettingsUsernameProvider : ISettingsUsernameProvider
        {
            return configuration.
                RegisterProviderBasedSettings<TSettingsSerializer, TSettingsStorageProvider>().
                RegisterSingleton<ISettingsUsernameProvider, TSettingsUsernameProvider>().
                RegisterSingleton<IUserSettings, ProviderBasedUserSettings>();
        }
    }
}