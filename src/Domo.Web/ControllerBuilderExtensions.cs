using System.Web.Mvc;
using Domo.DI;

namespace Domo.Web
{
    public static class ControllerBuilderExtensions
    {
        public static void SetDomoControllerFactory(this ControllerBuilder controllerBuilder, IContainer container)
        {
            var controllerFactory = container.ServiceLocator.Resolve<IControllerFactory>("Domo");

            controllerBuilder.SetControllerFactory(controllerFactory);
        }
    }
}