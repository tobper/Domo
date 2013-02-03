namespace Domo.DI.Registration
{
    public static class InstanceServiceExtensions
    {
        public static IService AsService<TService>(this TService instance, string serviceName = null)
        {
            var serviceType = typeof(TService);
            var identity = new ServiceIdentity(serviceType, serviceName);
            var service = new InstanceService(identity, instance);

            return service;
        }

        public static void RegisterIn<TService>(this TService instance, IContainer container, string serviceName = null)
        {
            var service = instance.AsService<TService>(serviceName);

            container.Register(service);
        }
    }
}