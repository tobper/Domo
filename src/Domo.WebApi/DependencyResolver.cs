using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Domo.DI;

namespace Domo.WebApi
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public DependencyResolver(IContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _container.ServiceLocator.TryResolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ServiceLocator.ResolveAll(serviceType);
        }

        public void Dispose()
        {
        }
    }
}