using System.Reflection;

namespace Domo.DI.Registration
{
    public interface IScanConvention
    {
        void ProcessType(IContainerConfiguration container, TypeInfo type);
    }
}