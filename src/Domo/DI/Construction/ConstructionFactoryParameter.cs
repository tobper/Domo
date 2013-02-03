using Domo.DI.Registration;

namespace Domo.DI.Construction
{
    public class ConstructionFactoryParameter
    {
        public IService Service { get; private set; }

        public ConstructionFactoryParameter(IService service)
        {
            Service = service;
        }
    }
}