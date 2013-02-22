namespace Domo.DI.Registration
{
    public static class BasicScanConventionsExtensions
    {
        public static IAssemblyScanner UseBasicConventions(this IAssemblyScanner scanner, bool usePrefixResolution = true)
        {
            var convention = new BasicScanConvention(usePrefixResolution);

            return scanner.UseConvention(convention);
        }
    }
}