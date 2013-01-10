using System.Web.Mvc;
using Domo.DI;

namespace Domo.Web
{
    public static class ControllerBuilderExtensions
    {
        public static void SetDomoControllerFactory(IContainer container)
        {
            var controllerFactory = container.ServiceLocator.Resolve<IControllerFactory>("Domo");

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}