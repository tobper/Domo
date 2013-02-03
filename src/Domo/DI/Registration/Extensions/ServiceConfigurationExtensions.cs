namespace Domo.DI.Registration
{
    public static class ServiceConfigurationExtensions
    {
        public static TService WithName<TService>(this TService service, string serviceName)
            where TService : IServiceConfiguration
        {
            service.WithName(serviceName);
            return service;
        }
    }
}