using Domo.DI.Activation;

namespace Domo.DI.Creation
{
    public interface IFactory
    {
        object CreateInstance(IInjectionContext context);
    }
}