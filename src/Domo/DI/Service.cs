using System;

namespace Domo.DI
{
    public class ServiceInfo
    {
        public Type ServiceType { get; private set; }
        public Type ActivationType { get; private set; }
        public LifeStyle LifeStyle { get; private set; }

        public ServiceInfo(Type serviceType, Type activationType, LifeStyle lifeStyle = LifeStyle.Default)
        {
            ServiceType = serviceType;
            ActivationType = activationType;
            LifeStyle = lifeStyle;
        }
    }
}