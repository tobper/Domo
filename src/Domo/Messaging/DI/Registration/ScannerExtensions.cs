using Domo.DI.Registration;

namespace Domo.Messaging.DI.Registration
{
    public static class ScannerExtensions
    {
        public static IAssemblyScanner UseMessageHandlerProcessor(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.AddScanProcessor(new MessageHandlerScanProcessor());
        }
    }
}