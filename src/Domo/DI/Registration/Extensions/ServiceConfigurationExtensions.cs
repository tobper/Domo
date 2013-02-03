namespace Domo.DI.Registration
{
    public static class ServiceConfigurationExtensions
    {
        public static TConfiguration WithName<TConfiguration>(this TConfiguration configuration, string serviceName)
            where TConfiguration : IServiceConfiguration
        {
            configuration.WithName(serviceName);
            return configuration;
        }
    }
}