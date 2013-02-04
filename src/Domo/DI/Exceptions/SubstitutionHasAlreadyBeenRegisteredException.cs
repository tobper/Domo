using System;

namespace Domo.DI
{
    public class SubstitutionHasAlreadyBeenRegisteredException : Exception
    {
        public SubstitutionHasAlreadyBeenRegisteredException(ServiceIdentity identity, Type existingType, Type concreteType)
            : base(CreateMessage(identity, existingType, concreteType))
        {
        }

        private static string CreateMessage(ServiceIdentity identity, Type existingType, Type concreteType)
        {
            return (identity.ServiceName == null)
                ? string.Format("Substitution registration of '{0}' for service type '{1}' failed because '{2}' has already been registered.", concreteType, identity.ServiceType, existingType)
                : string.Format("Substitution registration of '{0}' for service type '{1}' and name '{2}' failed because '{3}' has already been registered.", concreteType, identity.ServiceType, identity.ServiceName, existingType);
        }
    }
}