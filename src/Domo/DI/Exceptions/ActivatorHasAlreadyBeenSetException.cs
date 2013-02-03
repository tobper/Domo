using System;

namespace Domo.DI
{
    public class ActivatorHasAlreadyBeenSetException : Exception
    {
        public ActivatorHasAlreadyBeenSetException(ServiceIdentity identity, Type existingActivatorType, Type newActivatorType)
        {
            throw new NotImplementedException();
        }
    }
}