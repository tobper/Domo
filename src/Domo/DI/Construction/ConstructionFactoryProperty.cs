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

        public void Set(object instance, IInjectionContext context)
        {
            if (PropertyInfo.GetMethod.Invoke(instance, null) == null)
            {
                var propertyInstance = Activator.GetInstance(context);

                PropertyInfo.SetMethod.Invoke(instance, new[] { propertyInstance });
            }
        }
    }
}