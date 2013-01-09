using System;

namespace Domo.DI
{
    public class InvalidActivatorTypeException : Exception
    {
        protected Type ActivatorType { get; private set; }

        public InvalidActivatorTypeException(Type activatorType)
            : base(CreateMessage(activatorType))
        {
            ActivatorType = activatorType;
        }

        private static string CreateMessage(Type activatorType)
        {
            return string.Format("The type '{0}' does not implement the IActivator interface.", activatorType);
        }
    }
}