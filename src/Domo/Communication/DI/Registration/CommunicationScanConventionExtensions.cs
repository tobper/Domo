using InjectMe.Registration;

namespace Domo.Communication.DI.Registration
{
    public static class CommunicationScanConventionExtensions
    {
        public static IAssemblyScanner RegisterMessageHandlers(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseConvention<CommunicationScanConvention>();
        }
    }
}