using System;
using Domo.DI.Activation;

namespace Domo.DI
{
    public class InvalidScopeException : Exception
    {
        public InvalidScopeException(ActivationScope activationScope)
            : base(CreateMessage(activationScope))
        {
        }

        public InvalidScopeException(ActivationScope activationScope, Type serviceType)
            : base(CreateMessage(activationScope, serviceType))
        {
        }

        private static string CreateMessage(ActivationScope activationScope)
        {
            return string.Format("Invalid scope ({0}).", activationScope);
        }

        private static string CreateMessage(ActivationScope activationScope, Type serviceType)
        {
            return string.Format("Invalid scope ({0}) specified for service {1}.", activationScope, serviceType.Name);
        }
    }
}