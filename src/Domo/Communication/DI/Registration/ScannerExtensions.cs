using Domo.DI.Registration;

namespace Domo.Communication.DI.Registration
{
    public static class ScannerExtensions
    {
        public static IAssemblyScanner UseMessageHandlerProcessor(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseScanProcessor<MessageHandlerScanProcessor>();
        }
    }
}