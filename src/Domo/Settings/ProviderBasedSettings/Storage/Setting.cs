using System;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public class Setting
    {
        public Type Type { get; set; }
        public string Key { get; set; }
        public string User { get; set; }
        public object Value { get; set; }
        public Version Version { get; set; }
    }
}