using System;

namespace Domo.Settings
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SettingsAttribute : Attribute
    {
        public SettingsScope Scope { get; private set; }

        public SettingsAttribute(SettingsScope scope)
        {
            Scope = scope;
        }
    }
}