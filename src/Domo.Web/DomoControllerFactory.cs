using System.Web.Mvc;
using System.Web.Routing;
using Domo.DI;

namespace Domo.Web
{
    public class DomoControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public DomoControllerFactory(IContainer container)
        {
            _container = container;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return _container.ServiceLocator.Resolve<IController>(controllerName);
        }
    }
}
