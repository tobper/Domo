using Domo.DI.Registration;

namespace Domo.WebApi
{
    public static class ScannerExtensions
    {
        public static IAssemblyScanner UseApiControllerProcessor(this IAssemblyScanner assemblyScanner)
        {
            return assemblyScanner.UseScanProcessor<DomoApiControllerScanProcessor>();
        }
    }
}