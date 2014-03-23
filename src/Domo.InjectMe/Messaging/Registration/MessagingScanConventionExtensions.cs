using InjectMe.Registration;

namespace Domo.Messaging.Registration
{
    public static class MessagingScanConventionExtensions
    {
        public static IAssemblyScanner RegisterMessagingHandlers(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseConvention<MessagingScanConvention>();
        }
    }
}