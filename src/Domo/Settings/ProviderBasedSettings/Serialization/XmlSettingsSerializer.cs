using Domo.DI.Registration;
using Domo.Extensions;

namespace Domo.Settings.ProviderBasedSettings.Serialization
{
    [PreventAutomaticRegistration]
    public class XmlSettingsSerializer : SettingsSerializer<string>
    {
        protected override string Serialize<TValue>(TValue value)
        {
            return value.ToXml();
        }

        protected override TValue Deserialize<TValue>(string data)
        {
            return data.ParseXml<TValue>();
        }
    }
}