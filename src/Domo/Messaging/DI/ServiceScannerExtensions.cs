using Domo.DI.Registration;

namespace Domo.Messaging.DI
{
    public static class ServiceScannerExtensions
    {
        public static IAssemblyScanner UseMessageHandlerScanner(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.AddScanProcessor(new MessageHandlerScanProcessor());
        }
    }
}