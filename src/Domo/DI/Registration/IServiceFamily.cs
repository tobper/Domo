using System;

namespace Domo.DI.Registration
{
    public interface IServiceFamily
    {
        Type ServiceType { get; }

        void Add(IService service);
        IService GetService(ServiceIdentity identity);
        IService[] GetAllServices();
    }
}