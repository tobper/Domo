namespace Domo.DI.Registration
{
    public static class InstanceConfigurationExtensions
    {
        public static IContainerConfiguration Register<TService>(this IContainerConfiguration container, TService instance, string serviceName = null)
            where TService : class
        {
            var serviceType = typeof(TService);
            var identity = new ServiceIdentity(serviceType, serviceName);
            var configuration = new InstanceServiceConfiguration(identity, instance);

            return container.Register(configuration);
        }
    }
}