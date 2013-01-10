using System.Web.Http;
using System.Web.Http.Dependencies;
using Domo.DI;

namespace Domo.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static void SetDomoDependencyResolver(IContainer container)
        {
            var dependencyResolver = container.ServiceLocator.Resolve<IDependencyResolver>("Domo");

            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
        }
    }
}