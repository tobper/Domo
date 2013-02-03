using Domo.DI.Activation;

namespace Domo.DI.Construction
{
    public interface IFactoryContainer
    {
        void AddFactory(ServiceIdentity identity, IFactory factory);
        object CreateInstance(ServiceIdentity identity);
        object CreateInstance(ServiceIdentity identity, IInjectionContext context);
    }
}