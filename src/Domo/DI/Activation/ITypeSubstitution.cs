using System;

namespace Domo.DI.Activation
{
    public interface ITypeSubstitution
    {
        void AddSubstitution(ServiceIdentity identity, Type realServiceType);
        Type TryGetSubstitutedType(ServiceIdentity identity);
    }
}