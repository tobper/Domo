using Domo.DI.Registration;

namespace Domo.Communication.DI.Registration
{
    public static class ScannerExtensions
    {
        public static IAssemblyScanner UseCommunicationsProcessor(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseScanProcessor<CommunicationsScanProcessor>();
        }
    }
}