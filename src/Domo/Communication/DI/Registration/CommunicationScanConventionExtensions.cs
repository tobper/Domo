using Domo.DI.Registration;

namespace Domo.Communication.DI.Registration
{
    public static class CommunicationScanConventionExtensions
    {
        public static IAssemblyScanner UseCommunicationConventions(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseConvention<CommunicationScanConvention>();
        }
    }
}