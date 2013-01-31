using System;

namespace Domo.DI.Activation
{
    public interface ITypeSubstitution
    {
        void AddConcreteType(ServiceIdentity identity, Type concreteType);
        Type TryGetConcreteType(ServiceIdentity identity);
    }
}