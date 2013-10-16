using System;

namespace Domo.Settings
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class UserSettingsAttribute : SettingsAttribute
    {
        public UserSettingsAttribute()
            : base(SettingsScope.User)
        { }
    }
}