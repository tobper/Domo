using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public interface IServiceFamily
    {
        Type ServiceType { get; }

        void Add(IService service);
        IService GetService(ServiceIdentity identity);
        IEnumerable<IService> GetAllServices();
    }
}