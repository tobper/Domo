using System;
using System.Collections.Generic;
using Domo.Extensions;

namespace Domo.DI.Activation
{
    public class TypeSubstitution : ITypeSubstitution
    {
        private readonly IDictionary<ServiceIdentity, Type> _concreteTypes = new Dictionary<ServiceIdentity, Type>();

        public void AddConcreteType(ServiceIdentity identity, Type concreteType)
        {
            _concreteTypes.Add(identity, concreteType);
        }

        public Type TryGetConcreteType(ServiceIdentity identity)
        {
            return _concreteTypes.TryGetValue(identity);
        }
    }
}