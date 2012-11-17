using Domo.DI.Registration;
using Domo.Settings.DI;
using Domo.Settings.DI.Registration;
using Domo.Settings.ProviderBasedSettings;
using Domo.Settings.ProviderBasedSettings.Serialization;
using Domo.Settings.ProviderBasedSettings.Storage;

namespace Domo.Settings
{
    public static class ServiceScannerExtensions
    {
        /// <summary>
        /// The SettingsScanner will automatically register all types marked with <see cref="SettingsAttribute"/>.
        /// </summary>
        public static IAssemblyScanner UseSettingsScanner(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.AddScanProcessor(new SettingsScanProcessor());
        }

        public static ITypeRegistration RegisterProviderBasedSettings<TSerializer, TStorageProvider>(this ITypeRegistration typeRegistration)
            where TSerializer : ISettingsSerializer
            where TStorageProvider : ISettingsStorageProvider
        {
            return typeRegistration.
                Register<ISettingsSerializer, TSerializer>().
                Register<ISettingsStorageProvider, TStorageProvider>().
                Register<IApplicationSettings, ProviderBasedApplicationSettings>();
        }

        public static ITypeRegistration RegisterProviderBasedSettings<TSerializer, TStorageProvider, TUsernameProvider>(this ITypeRegistration typeRegistration)
            where TSerializer : ISettingsSerializer
            where TStorageProvider : ISettingsStorageProvider
            where TUsernameProvider : ISettingsUsernameProvider
        {
            return typeRegistration.
                RegisterProviderBasedSettings<TSerializer, TStorageProvider>().
                Register<ISettingsUsernameProvider, TUsernameProvider>().
                Register<IUserSettings, ProviderBasedUserSettings>();
        }
    }
}