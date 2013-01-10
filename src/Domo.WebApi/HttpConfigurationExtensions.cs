using System.Web.Http;
using System.Web.Http.Dependencies;
using Domo.DI;

namespace Domo.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static void SetDomoDependencyResolver(this HttpConfiguration httpConfiguration, IContainer container)
        {
            var dependencyResolver = container.ServiceLocator.Resolve<IDependencyResolver>("Domo");

            httpConfiguration.DependencyResolver = dependencyResolver;
        }
    }
}