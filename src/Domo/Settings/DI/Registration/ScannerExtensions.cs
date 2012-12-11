using Domo.DI.Registration;
using Domo.Settings.ProviderBasedSettings;
using Domo.Settings.ProviderBasedSettings.Serialization;
using Domo.Settings.ProviderBasedSettings.Storage;

namespace Domo.Settings.DI.Registration
{
    public static class ScannerExtensions
    {
        /// <summary>
        /// The SettingsScanner will automatically register all types marked with <see cref="SettingsAttribute"/>.
        /// </summary>
        public static IAssemblyScanner UseSettingsProcessor(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseScanProcessor<SettingsScanProcessor>();
        }

        public static ITypeRegistration RegisterProviderBasedSettings<TSettingsSerializer, TSettingsStorageProvider>(this ITypeRegistration typeRegistration)
            where TSettingsSerializer : ISettingsSerializer
            where TSettingsStorageProvider : ISettingsStorageProvider
        {
            return typeRegistration.
                Register<ISettingsSerializer, TSettingsSerializer>().
                Register<ISettingsStorageProvider, TSettingsStorageProvider>().
                Register<IApplicationSettings, ProviderBasedApplicationSettings>();
        }

        public static ITypeRegistration RegisterProviderBasedSettings<TSettingsSerializer, TSettingsStorageProvider, TSettingsUsernameProvider>(this ITypeRegistration typeRegistration)
            where TSettingsSerializer : ISettingsSerializer
            where TSettingsStorageProvider : ISettingsStorageProvider
            where TSettingsUsernameProvider : ISettingsUsernameProvider
        {
            return typeRegistration.
                RegisterProviderBasedSettings<TSettingsSerializer, TSettingsStorageProvider>().
                Register<ISettingsUsernameProvider, TSettingsUsernameProvider>().
                Register<IUserSettings, ProviderBasedUserSettings>();
        }
    }
}