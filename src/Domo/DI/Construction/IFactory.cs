using Domo.DI.Activation;

namespace Domo.DI.Construction
{
    public interface IFactory
    {
        object CreateService(IInjectionContext context);
    }
}