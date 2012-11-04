using Domo.DI.Scanning;

namespace Domo.Messaging.DI
{
    public static class ServiceScannerExtensions
    {
        public static IAssemblyScanner UseMessageHandlerScanner(this IAssemblyScanner builder)
        {
            return builder.AddServiceScanner(new MessageHandlerServiceScanner());
        }
    }
}