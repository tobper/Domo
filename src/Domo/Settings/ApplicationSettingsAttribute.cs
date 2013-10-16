using System;

namespace Domo.Settings
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ApplicationSettingsAttribute : SettingsAttribute
    {
        public ApplicationSettingsAttribute()
            : base(SettingsScope.Application)
        { }
    }
}