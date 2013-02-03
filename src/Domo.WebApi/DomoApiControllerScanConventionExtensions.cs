using Domo.DI.Registration;

namespace Domo.WebApi
{
    public static class DomoApiControllerScanConventionExtensions
    {
        public static IAssemblyScanner UseApiControllerConventions(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseConvention<DomoApiControllerScanConvention>();
        }
    }
}