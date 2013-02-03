using System;
using Domo.DI.Activation;

namespace Domo.DI
{
    public class InvalidScopeException : Exception
    {
        public InvalidScopeException(Scope scope)
            : base(CreateMessage(scope))
        {
        }

        public InvalidScopeException(Scope scope, Type serviceType)
            : base(CreateMessage(scope, serviceType))
        {
        }

        private static string CreateMessage(Scope scope)
        {
            return string.Format("Invalid scope ({0}).", scope);
        }

        private static string CreateMessage(Scope scope, Type serviceType)
        {
            return string.Format("Invalid scope ({0}) specified for service {1}.", scope, serviceType.Name);
        }
    }
}