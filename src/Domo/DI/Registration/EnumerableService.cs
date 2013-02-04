using System;
using System.Collections;
using System.Collections.Generic;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class EnumerableService : IService
    {
        private readonly ICollection<IService> _services;

        public EnumerableService(ServiceIdentity identity, ICollection<IService> services)
        {
            _services = services;
            Identity = identity;
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            var listItemType = Identity.ServiceType.GenericTypeArguments[0];
            var listType = typeof(List<>).MakeGenericType(listItemType);
            var list = (IList)Activator.CreateInstance(listType);

            foreach (var service in _services)
            {
                var instance = service.GetInstance(context);

                list.Add(instance);
            }

            return list;
        }
    }
}