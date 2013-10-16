using InjectMe.Registration;

namespace Domo.Communication.Registration
{
    public static class CommunicationScanConventionExtensions
    {
        public static IAssemblyScanner RegisterCommunicationHandlers(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseConvention<CommunicationScanConvention>();
        }
    }
}