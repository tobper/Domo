using System.Reflection;

namespace Domo.DI.Registration
{
    public interface IScanProcessor
    {
        void ProcessType(ITypeRegistration typeRegistration, TypeInfo type);
    }
}