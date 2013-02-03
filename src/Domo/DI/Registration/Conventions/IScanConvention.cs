using System.Reflection;

namespace Domo.DI.Registration.Conventions
{
    public interface IScanConvention
    {
        void ProcessType(IContainerConfiguration container, TypeInfo type);
    }
}