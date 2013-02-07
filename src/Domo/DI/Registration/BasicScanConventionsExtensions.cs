namespace Domo.DI.Registration
{
    public static class BasicScanConventionsExtensions
    {
        public static IAssemblyScanner UseBasicConventions(this IAssemblyScanner scanner)
        {
            return scanner.UseConvention<BasicScanConvention>();
        }
    }
}