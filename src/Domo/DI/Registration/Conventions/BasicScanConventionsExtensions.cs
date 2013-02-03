namespace Domo.DI.Registration.Conventions
{
    public static class BasicScanConventionsExtensions
    {
        public static IAssemblyScanner UseBasicConventions(this IAssemblyScanner scanner)
        {
            return scanner.UseConvention<BasicScanConvention>();
        }
    }
}