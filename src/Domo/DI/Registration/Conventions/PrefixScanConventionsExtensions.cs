namespace Domo.DI.Registration.Conventions
{
    public static class PrefixScanConventionsExtensions
    {
        public static IAssemblyScanner UsePrefixConventions(this IAssemblyScanner scanner)
        {
            return scanner.UseConvention<PrefixScanConvention>();
        }
    }
}