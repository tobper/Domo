using Domo.DI.Registration;
using Domo.Extensions;

namespace Domo.Settings.ProviderBasedSettings.Serialization
{
    [PreventAutomaticRegistration]
    public class JsonSettingsSerializer : SettingsSerializer<string>
    {
        protected override string Serialize<TValue>(TValue value)
        {
            return value.ToJson();
        }

        protected override TValue Deserialize<TValue>(string data)
        {
            return data.ParseJson<TValue>();
        }
    }
}