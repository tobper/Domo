using System.Reflection;
using Domo.DI.Activation;

namespace Domo.DI.Construction
{
    public class ConstructionFactoryProperty
    {
        public IActivator Activator { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        public ConstructionFactoryProperty(IActivator activator, PropertyInfo propertyInfo)
        {
            Activator = activator;
            PropertyInfo = propertyInfo;
        }

        public void Set(object service, IInjectionContext context)
        {
            if (PropertyInfo.GetMethod.Invoke(service, null) == null)
            {
                var propertyService = Activator.ActivateService(context);

                PropertyInfo.SetMethod.Invoke(service, new[] { propertyService });
            }
        }
    }
}