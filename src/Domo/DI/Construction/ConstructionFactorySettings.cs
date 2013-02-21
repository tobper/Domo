using System;

namespace Domo.DI.Construction
{
    public class ConstructionFactorySettings
    {
        public Type PropertyInjectionAttribute { get; set; }
        public bool UsePropertyInjection { get; set; }
        public bool UsePrefixResolution { get; set; }
    }
}